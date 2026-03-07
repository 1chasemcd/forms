import { computed, Signal } from '@angular/core';
import { BaseField, BaseInput, PropertyOrConstant } from '../api/api.g';

export type FormModel = Record<string, unknown>;

export function computedPropertyOrConstant<T>(
  pc: Signal<PropertyOrConstant | undefined>,
  model: Signal<FormModel | undefined>,
): Signal<T | undefined> {
  return computed(() => {
    const val = pc();
    if (val?.$type == 'constant') return val.Value;
    if (val?.$type == 'property' && val.Value) {
      const m = model();
      if (m) return m[val.Value];
    }
    return undefined;
  });
}

const colSpanMap: Record<number, string> = {
  1: 'col-span-1',
  2: 'col-span-2',
  3: 'col-span-3',
  4: 'col-span-4',
  5: 'col-span-5',
  6: 'col-span-6',
  7: 'col-span-7',
  8: 'col-span-8',
  9: 'col-span-9',
  10: 'col-span-10',
  11: 'col-span-11',
  12: 'col-span-12',
};

export function widthToCss(width?: number): string {
  if (!width || width < 1 || width > 12) width = 12;
  return colSpanMap[width];
}

type BaseInputType = BaseInput['$type'];
const baseInputTypes: Record<BaseInputType, true> = {
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
