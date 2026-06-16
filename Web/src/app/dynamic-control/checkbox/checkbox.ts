import { Component, input, output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';

@Component({
  selector: 'app-checkbox',
  imports: [ReactiveFormsModule, MatCheckboxModule],
  templateUrl: './checkbox.html',
})
export class Checkbox {
  readonly label = input.required<string>();
  readonly control = input.required<FormControl>();
  readonly checkedChange = output();
}
