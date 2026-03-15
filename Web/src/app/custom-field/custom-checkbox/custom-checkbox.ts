import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-custom-checkbox',
  imports: [ReactiveFormsModule],
  templateUrl: './custom-checkbox.html',
})
export class CustomCheckbox {
  readonly label = input.required<string>();
  readonly control = input.required<FormControl>();
}
