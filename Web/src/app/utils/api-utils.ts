import { BaseField, BaseInput } from '../api/api.g';

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
