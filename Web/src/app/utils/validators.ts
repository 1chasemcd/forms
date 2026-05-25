import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function createPrecisionValidator() {
  let precision: number | null = null;

  const validator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    if (control.value == null || control.value === '') return null;

    const totalDigits = control.value.toString().replace('-', '').replace('.', '').length;

    if (precision != null && totalDigits > precision)
      return { precision: { requiredPrecision: precision, actual: totalDigits } };
    return null;
  };

  return {
    validator,
    setPrecision: (value: number | null) => (precision = value),
  };
}

export function createScaleValidator() {
  let scale: number | null = null;

  const validator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    if (control.value == null || control.value === '') return null;

    const [, decimalPart = ''] = control.value.toString().split('.');

    if (scale != null && decimalPart.length > scale)
      return { scale: { requiredScale: scale, actual: decimalPart.length } };
    return null;
  };

  return {
    validator,
    setScale: (value: number | null) => (scale = value),
  };
}

export function createMinValueValidator<T = string | number>() {
  let min: T | null = null;

  const validator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (value === null || value === '') return null;

    if (min != null && value < min) return { minValue: { requiredMin: min, actual: value } };

    return null;
  };

  return {
    validator,
    setMin: (v: T | null) => (min = v),
  };
}

export function createMaxValueValidator<T = string | number>() {
  let max: T | null = null;

  const validator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (value === null || value === '') return null;

    if (max != null && value > max) return { maxValue: { requiredMax: max, actual: value } };

    return null;
  };

  return {
    validator,
    setMax: (v: T | null) => (max = v),
  };
}

export function createMaxLengthValidator() {
  let maxLength: number | null = null;

  const validator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (value === null || value === '') return null;

    if (maxLength != null && value.length > maxLength) {
      return { maxLength: { requiredMaxLength: maxLength, actual: value.length } };
    }
    return null;
  };

  return {
    validator,
    setMaxLength: (v: number | null) => (maxLength = v),
  };
}
