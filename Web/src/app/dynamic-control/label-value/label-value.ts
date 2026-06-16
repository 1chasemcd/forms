import { Component, input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-label-value',
  templateUrl: './label-value.html',
  styleUrl: './label-value.css',
})
export class CustomLabelValue {
  readonly label = input.required<string>();
  readonly control = input.required<FormControl>();
}
