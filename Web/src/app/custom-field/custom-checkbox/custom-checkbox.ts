import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';

@Component({
  selector: 'app-custom-checkbox',
  imports: [ReactiveFormsModule],
  templateUrl: './custom-checkbox.html',
})
export class CustomCheckbox implements CustomInputComponent {
  readonly label = input.required<string>();
  readonly formControl = input.required<FormControl>();

  get required() {
    return this.formControl().hasValidator(Validators.required);
  }

  get disabled() {
    return this.formControl().disabled;
  }
}
