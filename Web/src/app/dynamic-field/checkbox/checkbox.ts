import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-checkbox',
  imports: [ReactiveFormsModule],
  templateUrl: './checkbox.html',
})
export class Checkbox {
  readonly label = input.required<string>();
  readonly control = input.required<FormControl>();
}
