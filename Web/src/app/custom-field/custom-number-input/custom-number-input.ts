import { Component, computed, input } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
import { CustomInputBase } from '../custom-input-base/custom-input-base';

@Component({
  selector: 'app-custom-number-input',
  imports: [CustomInputBase, ReactiveFormsModule],
  template: `
    <app-custom-input-base
      [label]="label()"
      [isRequired]="isRequired()"
      [formControl]="formControl()"
      [textAlign]="'right'"
      [controlToDisplay]="controlToDisplay"
      [displayToControl]="displayToControl"
      [transformDisplayOnChange]="onInput"
    ></app-custom-input-base>
  `,
})
export class CustomNumberInput implements CustomInputComponent {
  label = input.required<string>();
  formControl = input.required<FormControl<number | null>>();

  readonly isRequired = computed(() => this.formControl().hasValidator(Validators.required));

  controlToDisplay = (value: number | null): string => {
    if (value === null || value === undefined) return '';
    return this.formatNumber(value);
  };

  displayToControl = (value: string): number | null => {
    if (!value) return null;

    const numeric = value.replace(/,/g, '');
    const parsed = Number(numeric);

    return isNaN(parsed) ? null : parsed;
  };

  onInput = (value: string): string => {
    const numeric = value.replace(/,/g, '');
    const parsed = Number(numeric);
    return this.formatNumber(parsed);
  };

  private formatNumber(value: number): string {
    return new Intl.NumberFormat('en-US').format(value);
  }
}
