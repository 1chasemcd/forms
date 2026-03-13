import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-checkbox-field',
  imports: [ReactiveFormsModule],
  templateUrl: './checkbox-field.html',
})
export class CheckboxField {
  readonly label = input.required<string>();
  readonly formControl = input.required<FormControl>();
}
