import { Observable, of, startWith } from 'rxjs';
import { FieldType, FormValueRef, ServiceMethod } from '../../api/api.g';
import { MetadataLookupService } from '../../metadata/metadata-lookup';
import { MetadataType } from '../../utils/api-utils';
import { ControlPath, joinPath, parentPath } from '../../utils/form-utils';
import { FormModel } from './form-model';

export class ValueRefAugmentor {
  constructor(
    private readonly model: FormModel,
    private readonly metadata: MetadataLookupService,
  ) {}

  getValue<T>(
    relativeTo: ControlPath,
    valueRef: FormValueRef | undefined,
  ): Observable<T> | undefined {
    if (!valueRef) return undefined;
    if (valueRef.$type === 'constant') return of(valueRef.value);
    const control = this.model.get(joinPath(relativeTo, valueRef.value));
    return control?.valueChanges.pipe(startWith(control.value));
  }

  getMetadataValue(path: ControlPath, metadataType: 'fieldType'): FieldType | undefined;
  getMetadataValue(path: ControlPath, metadataType: 'formServiceMethod'): ServiceMethod | undefined;
  getMetadataValue<T>(
    path: ControlPath,
    metadataType: Exclude<MetadataType, 'fieldType' | 'formServiceMethod'>,
  ): Observable<T> | undefined;
  getMetadataValue<T>(
    path: ControlPath,
    metadataType: MetadataType,
  ): Observable<T> | ServiceMethod | FieldType | undefined {
    if (metadataType === 'fieldType' || metadataType === 'formServiceMethod') {
      return this.metadata.getPropertyMetadata(this.model.root, path, metadataType);
    }
    const valueRef = this.metadata.getPropertyMetadata(this.model.root, path, metadataType);
    if (!valueRef) return undefined;
    return this.getValue<T>(parentPath(path), valueRef);
  }
}
