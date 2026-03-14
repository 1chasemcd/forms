import { Component, input } from '@angular/core';

@Component({
  selector: 'app-button-field',
  imports: [],
  templateUrl: './button-field.html',
})
export class ButtonComponent {
  readonly label = input.required<string>();
  readonly disabled = input<boolean>();
  readonly onClick = input.required<() => void>();
}
