import { FieldDefinition, MetadataType, PropertyOrConstant } from '../api/api.g';
import { pascalCaseToWords } from './string-utils';
import { FormContext } from '../dynamic-form/form-context';
import { Observable, of, startWith } from 'rxjs';

export function applyPropertyOrConstant<T>(
  poc: PropertyOrConstant | null | undefined,
  context: FormContext,
  callback: (value: T) => void,
) {
  if (poc === null || poc === undefined) return;
  if (poc.$type === 'constant') callback(poc.value);
  else {
    const propertyControl = context.getOrAddControl(poc.value);
    propertyControl?.valueChanges.subscribe(callback);
  }
}

export function getPocObservable(
  poc: PropertyOrConstant,
  context: FormContext,
): Observable<unknown> {
  if (poc.$type === 'constant') return of(poc.value);
  const control = context.getOrAddControl(poc.value);
  return control.valueChanges.pipe(startWith(control.value));
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
