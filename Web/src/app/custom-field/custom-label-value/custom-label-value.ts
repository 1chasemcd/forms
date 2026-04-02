import { Component, input } from '@angular/core';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-custom-label-value',
  imports: [],
  templateUrl: './custom-label-value.html',
})
export class CustomLabelValue implements CustomInputComponent {
  readonly label = input.required<string>();
  readonly formControl = input.required<FormControl>();
}
