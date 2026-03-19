import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function createPrecisionScaleValidator() {
  let precision: number | null = null;
  let scale: number | null = null;

  const validator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    if (control.value == null || control.value === '') return null;
    const errors: ValidationErrors = {};

    const [integerPart, decimalPart = ''] = control.value.toString().split('.');

    const totalDigits = integerPart.replace('-', '').length + decimalPart.length;
    if (precision != null && totalDigits > precision)
      errors['precision'] = {
        requiredPrecision: precision,
        actualPrecision: totalDigits,
      };
    if (scale != null && decimalPart.length > scale)
      errors['scale'] = {
        requiredScale: scale,
        actualScale: decimalPart.length,
      };
    return Object.keys(errors).length ? errors : null;
  };

  return {
    validator,
    setPrecision: (value: number | null) => (precision = value),
    setScale: (value: number | null) => (scale = value),
  };
}

export function createRangeValidator<T = string | number>() {
  let min: T | null = null;
  let max: T | null = null;

  const validator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (value === null || value === '') return null;

    if (min != null && value < min) {
      return { minValue: { requiredMin: min, actual: value } };
    }

    if (max != null && value > max) {
      return { maxValue: { requiredMax: max, actual: value } };
    }

    return null;
  };

  return {
    validator,
    setMin: (v: T | null) => (min = v),
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
