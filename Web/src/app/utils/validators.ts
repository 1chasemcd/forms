import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function precisionScaleValidator(precision: number, scale: number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (control.value == null || control.value === '') return null;

    const value = control.value.toString();

    const [integerPart, decimalPart = ''] = value.split('.');

    const totalDigits = integerPart.replace('-', '').length + decimalPart.length;

    if (decimalPart.length > scale) {
      return { scale: { requiredScale: scale, actualScale: decimalPart.length } };
    }

    if (totalDigits > precision) {
      return { precision: { requiredPrecision: precision, actualPrecision: totalDigits } };
    }

    return null;
  };
}
