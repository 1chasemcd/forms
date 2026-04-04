import { Component, input, output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-checkbox-input',
  imports: [ReactiveFormsModule],
  templateUrl: './checkbox-input.html',
})
export class CheckboxInput {
  readonly control = input.required<FormControl>();
  readonly recalculateEvent = output();
}
