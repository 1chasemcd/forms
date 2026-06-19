import { inject, Injectable } from '@angular/core';
import { MetadataProcessorRegistryService } from '../../metadata/metadata-processor-registry-service';
import { MetadataLookupService } from '../../metadata/metadata-lookup-service';
import { AbstractControl, FormArray, FormControl, FormGroup } from '@angular/forms';
import {
  ControlPath,
  isControlPath,
  joinPath,
  lastOfPath,
  parentPath,
} from '../../utils/form-utils';

@Injectable()
export class FormModelService {
  private metadataProcessorRegistry = inject(MetadataProcessorRegistryService);
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
      if (metadataContainer.$type === 'enumerable')
        formGroup.addControl(propertyName, new FormArray([]));
      else if (metadataContainer.$type === 'subproperty')
        formGroup.addControl(propertyName, this.createFormGroup(metadataContainer.subPropertyType));
      else formGroup.addControl(propertyName, new FormControl(''));
    }

    for (const [propertyName, metadataContainer] of Object.entries(
      rootMetadata.propertyMetadatas,
    )) {
      if (metadataContainer.$type !== 'primitive') continue;
      const control = formGroup.get(propertyName);
      if (!control) continue;
      metadataContainer.metadatas.forEach((m) =>
        this.metadataProcessorRegistry.getMetadataProcessor(m)?.process(control, m),
      );
    }

    return formGroup;
  }

  get<T extends AbstractControl>(path: ControlPath): T | null {
    if (!isControlPath(path)) return null;
    if (path.length == 0) return this._model as unknown as T;
    return this._model.get(path) as T;
  }

  private getOrAdd<T extends AbstractControl>(path: ControlPath, toAdd: T): T {
    const existing = this.get<T>(path);
    if (existing) return existing;
    const lastKey = lastOfPath(path);
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
    for (const [key, value] of Object.entries(values)) {
      const propPath = joinPath(path, key);
      const propMetadata = this.metadataLookup.lookupByPath(propPath);
      if (propMetadata?.$type === 'enumerable') {
        this.patchArrayValues(
          value as Record<string, unknown>[],
          propPath,
          propMetadata.enumeratedType,
        );
      } else if (propMetadata?.$type === 'subproperty') {
        this.patchValues(propPath, value as Record<string, unknown>);
      } else {
        this.getOrAdd(propPath, new FormControl(value));
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
      this.patchValues(joinPath(path, index), row);
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
