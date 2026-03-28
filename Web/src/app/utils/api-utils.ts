import { FormGroup } from '@angular/forms';
import { BaseField, BaseInput, PropertyOrConstant } from '../api/api.g';

export type BaseFieldType = BaseField['$type'];
export type BaseInputType = BaseInput['$type'];
export const baseInputTypes: Record<BaseInputType, true> = {
  checkboxinput: true,
  textinput: true,
  textareainput: true,
  currencyinput: true,
  numericinput: true,
  dateinput: true,
  timeinput: true,
};

export function isBaseInput(field?: BaseField): field is BaseInput {
  return field != undefined && field.$type in baseInputTypes;
}

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
