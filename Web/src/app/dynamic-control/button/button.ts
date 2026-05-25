import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.html',
})
export class Button {
  readonly label = input.required<string>();
  readonly enabled = input<boolean>(true);
  readonly clicked = output();
}
