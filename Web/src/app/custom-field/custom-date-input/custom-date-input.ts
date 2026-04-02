import { Component, computed, input } from '@angular/core';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomInputBase } from '../custom-input-base/custom-input-base';

@Component({
  selector: 'app-custom-date-input',
  imports: [ReactiveFormsModule, CustomInputBase],
  template: `
    <app-custom-input-base
      [label]="label()"
      [isRequired]="isRequired()"
      [formControl]="formControl()"
      inputType="date"
    ></app-custom-input-base>
  `,
})
export class CustomDateInput implements CustomInputComponent {
  formControl = input.required<FormControl>();
  readonly label = input.required<string>();
  readonly isRequired = computed(() => this.formControl().hasValidator(Validators.required));
}
