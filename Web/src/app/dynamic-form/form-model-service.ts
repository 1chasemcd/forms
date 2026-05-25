import { inject, Injectable } from '@angular/core';
import { MetadataProcessorRegistryService } from '../metadata/metadata-processor-registry-service';
import { FormFieldEnablementService } from '../form-processor/form-field-enablement-service';
import { MetadataLookupService } from '../metadata/metadata-lookup-service';
import { AbstractControl, FormArray, FormGroup } from '@angular/forms';
import { getOrAddArray, getOrAddControl, getOrAddGroup } from '../utils/api-utils';
import { ControlPath, parentPath } from '../utils/form-utils';

@Injectable()
export class FormModelService {
  private metadataProcessorRegistry = inject(MetadataProcessorRegistryService);
  private enablementService = inject(FormFieldEnablementService);
  private metadataLookup = inject(MetadataLookupService);

  private _model: FormGroup = new FormGroup({});
  get model() {
    return this._model;
  }

  initialize() {
    if (!this.metadataLookup.isInitialized)
      throw Error('Must initialize MetadataLookupService before initializing FormModelService');
    const rootType = this.metadataLookup.rootType;
    this._model = this.createFormGroup(rootType);
  }

  private createFormGroup(type: string): FormGroup {
    const formGroup = new FormGroup({});
    const rootMetadata = this.metadataLookup.lookupByType(type);
    if (!rootMetadata) return formGroup;

    for (const [propertyName, metadataContainer] of Object.entries(
      rootMetadata.propertyMetadatas,
    )) {
      if (metadataContainer.$type === 'enumerable') getOrAddArray(formGroup, propertyName);
      else if (metadataContainer.$type === 'subproperty') {
        const group = this.createFormGroup(metadataContainer.subPropertyType);
        getOrAddGroup(formGroup, propertyName, group);
      } else {
        const control = getOrAddControl(formGroup, propertyName);
        this.enablementService.registerControl(control);
        metadataContainer.metadatas.forEach((m) =>
          this.metadataProcessorRegistry.getMetadataProcessor(m)?.process(control, formGroup, m),
        );
      }
    }

    return formGroup;
  }

  get<T extends AbstractControl>(path: ControlPath) {
    return this._model.get(path) as T;
  }

  getOrAdd<T extends AbstractControl>(path: ControlPath, toAdd: T): T {
    const existing = this._model.get(path) as T;
    if (existing) return existing;
    const lastKey = path.at(-1) as unknown as string | number; // if we make it here path is not empty
    if (typeof lastKey === 'number') {
      const parent = this.getOrAdd(parentPath(path), new FormArray<T>([]));
      parent.setControl(lastKey, toAdd);
    } else {
      const parent = this.getOrAdd(parentPath(path), new FormGroup({}));
      parent.addControl(lastKey, toAdd);
    }
    return toAdd;
  }

  patchValues(path: ControlPath, values: Record<string, unknown>) {
    const group = this.getOrAdd(path, new FormGroup({}));

    for (const [key, value] of Object.entries(values)) {
      const propPath = [...path, key];
      const propMetadata = this.metadataLookup.lookupByPath(propPath);
      if (!propMetadata) continue;
      if (propMetadata.$type === 'enumerable') {
        this.patchArrayValues(
          value as Record<string, unknown>[],
          propPath,
          propMetadata.enumeratedType,
        );
      } else if (propMetadata.$type === 'subproperty') {
        this.patchValues(propPath, value as Record<string, unknown>);
      } else {
        const toSet = getOrAddControl(group, key);
        toSet.setValue(value);
      }
    }
  }

  private patchArrayValues(
    valuesArray: Record<string, unknown>[],
    path: ControlPath,
    gridType: string,
  ) {
    const array = this.getOrAdd(path, new FormArray<FormGroup>([]));

    if (!Array.isArray(valuesArray) || valuesArray.length == 0) {
      array.clear();
      return;
    }

    array.clear();

    // TODO make this more efficient
    for (const [index, row] of valuesArray.entries()) {
      const rowGroup = this.createFormGroup(gridType);
      array.push(rowGroup);
      this.patchValues([...path, index], row);
    }
  }

  toRecord(path: ControlPath): Record<string, unknown> {
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
}
