import { FormControl, FormGroup } from '@angular/forms';
import { FieldDefinition, MetadataType, PropertyOrConstant } from '../api/api.g';
import { pascalCaseToWords } from './string-utils';

export function applyPropertyOrConstant<T>(
  poc: PropertyOrConstant | null | undefined,
  formGroup: FormGroup,
  callback: (value: T) => void,
) {
  if (poc === null || poc === undefined) return;
  if (poc.$type === 'constant') callback(poc.value);
  else {
    const propertyControl = getOrAddControl(poc.value, formGroup);
    propertyControl?.valueChanges.subscribe(callback);
  }
}

export function getMetadata<T>(field: FieldDefinition, type: MetadataType) {
  const metadata = field.fieldMetadatas?.find((x) => x.type == type);
  return metadata?.value as T | undefined;
}

export function getLabel(field: FieldDefinition): PropertyOrConstant {
  const metadataLabel = getMetadata<PropertyOrConstant>(field, MetadataType.Label);
  if (metadataLabel !== null && metadataLabel !== undefined) return metadataLabel;
  return {
    $type: 'constant',
    value: pascalCaseToWords(field.property),
  };
}

export function getOrAddControl(key: string, formGroup: FormGroup) {
  if (!formGroup.get(key)) formGroup.addControl(key, new FormControl({}));
  return formGroup.get(key) as FormControl;
}
