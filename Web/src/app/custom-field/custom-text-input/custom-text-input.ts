import { Component, computed, input } from '@angular/core';
import { CustomInputBase } from '../custom-input-base/custom-input-base';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-custom-text-input',
  imports: [CustomInputBase, ReactiveFormsModule],
  template: `<app-custom-input-base
    [label]="label()"
    [isRequired]="isRequired()"
    [formControl]="formControl()"
  ></app-custom-input-base>`,
})
export class CustomTextInput implements CustomInputComponent {
  label = input.required<string>();
  formControl = input.required<FormControl>();
  readonly isRequired = computed(() => this.formControl().hasValidator(Validators.required));
}
