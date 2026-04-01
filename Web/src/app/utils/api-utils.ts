import { FormGroup } from '@angular/forms';
import { FieldDefinition, MetadataType, PropertyOrConstant } from '../api/api.g';

export function applyPropertyOrConstant<T>(
  poc: PropertyOrConstant | null | undefined,
  formGroup: FormGroup,
  callback: (value: T) => void,
) {
  if (poc === null || poc === undefined) return;
  if (poc.$type === 'constant') callback(poc.Value);
  else {
    const propertyControl = formGroup.get(poc.Value);
    propertyControl?.valueChanges.subscribe(callback);
  }
}

export function getMetadata<T>(field: FieldDefinition, type: MetadataType) {
  const metadata = field.FieldMetadatas?.find((x) => x.Type == type);
  return metadata?.Value as T | undefined;
}
