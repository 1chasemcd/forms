import { Component, computed, input } from '@angular/core';
import { CustomInput } from '../custom-input/custom-input';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-custom-text-input',
  imports: [CustomInput, ReactiveFormsModule],
  template: `<app-custom-input
    [label]="label()"
    [isRequired]="isRequired()"
    [formControl]="formControl()"
  ></app-custom-input>`,
})
export class CustomTextInput implements CustomInputComponent {
  label = input.required<string>();
  formControl = input.required<FormControl>();
  readonly isRequired = computed(() => this.formControl().hasValidator(Validators.required));
}
