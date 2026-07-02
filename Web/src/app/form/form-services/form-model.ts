import { Injector } from '@angular/core';
import { MetadataProcessorRegistryService } from '../../metadata/metadata-processor-registry-service';
import { MetadataLookupService } from '../../metadata/metadata-lookup';
import { AbstractControl, FormArray, FormControl, FormGroup } from '@angular/forms';
import { ControlPath, joinPath } from '../../utils/form-utils';
import { ControlEnablementStore } from './control-enablement-store';
import { ValueRefAugmentor } from './value-ref-augmentor';

export class FormModel {
  private readonly metadataProcessorRegistry;
  private readonly metadataLookup;

  readonly root: string;
  readonly controlEnablements = new ControlEnablementStore(this);
  readonly valueRefAugmentor;
  readonly formGroup: FormGroup;

  constructor(
    root: string,
    private readonly injector: Injector,
  ) {
    this.metadataLookup = this.injector.get(MetadataLookupService);
    this.metadataProcessorRegistry = this.injector.get(MetadataProcessorRegistryService);
    this.valueRefAugmentor = new ValueRefAugmentor(this, this.metadataLookup);
    this.root = root;

    this.formGroup = this.createFormGroup(root);
    this.applyMetadata(this.formGroup, []);
  }

  private createFormGroup(type: string): FormGroup {
    const controls: Record<string, AbstractControl> = {};
    const rootMetadata = this.metadataLookup.lookupByType(type);
    if (!rootMetadata) return new FormGroup(controls);

    for (const [propertyName, metadataContainer] of Object.entries(
      rootMetadata.propertyMetadatas,
    )) {
      if (metadataContainer.$type === 'enumerable')
        controls[propertyName] = new FormArray<FormGroup>([]);
      else if (metadataContainer.$type === 'subproperty')
        controls[propertyName] = this.createFormGroup(metadataContainer.subPropertyType);
      else controls[propertyName] = new FormControl('');
    }

    return new FormGroup(controls);
  }

  private applyMetadata(control: AbstractControl, path: ControlPath): void {
    const metadataContainer = this.metadataLookup.lookupByPath(this.root, path);

    if (metadataContainer?.$type === 'primitive') {
      metadataContainer.metadatas.forEach((m) =>
        this.metadataProcessorRegistry.getMetadataProcessor(m)?.process({
          model: this,
          control: control,
          controlPath: path,
          metadata: m,
        }),
      );
    }

    if (control instanceof FormGroup)
      for (const [name, child] of Object.entries(control.controls))
        this.applyMetadata(child, joinPath(path, name));
    else if (control instanceof FormArray)
      for (const [index, child] of control.controls.entries())
        this.applyMetadata(child, joinPath(path, index));
  }

  get<T extends AbstractControl>(path: ControlPath): T | null {
    if (path.length == 0) return this.formGroup as unknown as T;
    return this.formGroup.get(path) as T;
  }

  patchValues(values: Record<string, unknown>, path: ControlPath = []) {
    const group = this.get<FormGroup>(path);
    if (!group) return;
    this.patchValuesImpl(values, group, path);
  }

  private patchValuesImpl(
    values: Record<string, unknown>,
    group: FormGroup,
    path: ControlPath = [],
  ) {
    for (const [key, value] of Object.entries(values)) {
      const propPath = joinPath(path, key);
      const propMetadata = this.metadataLookup.lookupByPath(this.root, propPath);
      let control = group.get(key);
      const alreadyExists = !!control;

      if (propMetadata?.$type === 'enumerable') {
        control ??= new FormArray([]);
        this.patchArrayValues(
          value as Record<string, unknown>[],
          control as FormArray,
          propPath,
          propMetadata.enumeratedType,
        );
      } else if (propMetadata?.$type === 'subproperty') {
        control ??= new FormGroup({});
        this.patchValuesImpl(value as Record<string, unknown>, control as FormGroup, propPath);
      } else {
        control ??= new FormControl();
        if (value !== control.value) control.setValue(value);
      }

      if (!alreadyExists) group.addControl(key, control);
    }
  }

  private patchArrayValues(
    valuesArray: Record<string, unknown>[],
    array: FormArray,
    path: ControlPath,
    tableType: string,
  ) {
    array.clear({ emitEvent: false });

    if (!Array.isArray(valuesArray) || valuesArray.length == 0) return;

    // TODO make this more efficient
    for (const [index, row] of valuesArray.entries()) {
      const rowPath = joinPath(path, index);
      const rowGroup = this.createFormGroup(tableType);
      this.patchValuesImpl(row, rowGroup, rowPath);
      array.push(rowGroup);
      this.applyMetadata(rowGroup, rowPath);
    }
  }

  toRecord(path: ControlPath = []): Record<string, unknown> {
    const model = this.get<FormGroup>(path);
    if (!model) return {};
    return this.toRecordImpl(model);
  }

  private toRecordImpl(formGroup: FormGroup): Record<string, unknown> {
    const result: Record<string, unknown> = {};

    for (const kv of Object.entries(formGroup.controls)) {
      if (kv[1] instanceof FormArray) result[kv[0]] = this.toArray(kv[1]);
      else result[kv[0]] = kv[1].value;
    }

    return result;
  }

  private toArray(formArray: FormArray<FormGroup>): Record<string, unknown>[] {
    const result = [];
    for (const group of formArray.controls) {
      result.push(this.toRecordImpl(group));
    }

    return result;
  }

  clone(): FormModel {
    const clone = new FormModel(this.root, this.injector);
    const asRecord = this.toRecord();
    clone.patchValues(asRecord);
    return clone;
  }
}
