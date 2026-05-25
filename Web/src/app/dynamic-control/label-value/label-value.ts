import { Component, input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-label-value',
  imports: [],
  templateUrl: './label-value.html',
})
export class CustomLabelValue {
  readonly label = input.required<string>();
  readonly control = input.required<FormControl>();
}
