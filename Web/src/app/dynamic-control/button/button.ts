import { Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-button',
  templateUrl: './button.html',
  imports: [MatButtonModule],
})
export class Button {
  readonly label = input.required<string>();
  readonly enabled = input<boolean>(true);
  readonly clicked = output();
}
