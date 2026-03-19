import { Component, computed, input } from '@angular/core';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomInput } from '../custom-input/custom-input';

@Component({
  selector: 'app-custom-date-input',
  imports: [CustomInput, ReactiveFormsModule],
  template: `
    <app-custom-input
      [label]="label()"
      [isRequired]="isRequired()"
      [formControl]="formControl()"
      [controlToDisplay]="controlToDisplay"
      [displayToControl]="displayToControl"
      [transformDisplayOnChange]="onInput"
    ></app-custom-input>
  `,
})
export class CustomDateInput implements CustomInputComponent {
  label = input.required<string>();
  formControl = input.required<FormControl<number | null>>();

  readonly isRequired = computed(() => this.formControl().hasValidator(Validators.required));

  controlToDisplay = (value: string | null): string => {
    if (value === null || value === undefined) return '';
    value = value.replace(/-/g, '').padEnd(8);
    return value.substring(4, 6) + '/' + value.substring(6, 8) + '/' + value.substring(0, 4);
  };

  displayToControl = (value: string): string | null => {
    if (value === null || value || '') return null;
    let result = '';
    const parts = value.split('/');
    if (parts.length >= 3) result += parts[3];
    result.padEnd(4);
    result += '-';
    if (parts.length >= 1) result += parts[1];
    result.padEnd(7);
    result += '-';
    if (parts.length >= 2) result += parts[2];
    result.padEnd(10);

    return result;
  };

  onInput = (value: string): string => {
    let result = value.replace(/[^\d]/g, '');
    if (result.length > 2) result = result.substring(0, 2) + '/' + result.substring(2);
    if (result.length > 5) result = result.substring(0, 5) + '/' + result.substring(5);
    return result.substring(0, 10);
  };
}
