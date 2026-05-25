import { Component, input, output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CheckboxInput } from './checkbox-input';

@Component({
  selector: 'app-checkbox',
  imports: [ReactiveFormsModule, CheckboxInput],
  templateUrl: './checkbox.html',
})
export class Checkbox {
  readonly label = input.required<string>();
  readonly control = input.required<FormControl>();
  readonly checkedChange = output();
}
