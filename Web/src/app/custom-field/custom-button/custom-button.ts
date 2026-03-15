import { Component, input } from '@angular/core';

@Component({
  selector: 'app-custom-button',
  imports: [],
  templateUrl: './custom-button.html',
})
export class CustomButton {
  readonly label = input.required<string>();
  readonly disabled = input<boolean>();
  readonly onClick = input.required<() => void>();
}
