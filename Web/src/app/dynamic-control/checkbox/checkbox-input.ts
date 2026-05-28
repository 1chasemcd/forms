import { Component, input, output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CheckboxIcon } from './checkbox-icon';

@Component({
  selector: 'app-checkbox-input',
  imports: [ReactiveFormsModule, CheckboxIcon],
  templateUrl: './checkbox-input.html',
  host: {
    class: 'flex items-center justify-center h-full',
  },
})
export class CheckboxInput {
  readonly control = input.required<FormControl>();
  readonly checkedChange = output();
}
