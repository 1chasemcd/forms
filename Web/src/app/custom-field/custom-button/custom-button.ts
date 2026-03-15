import { Component, input, output } from '@angular/core';
import { CustomButtonComponent } from '../../field-resolution/custom-field-registration';

@Component({
  selector: 'app-custom-button',
  imports: [],
  templateUrl: './custom-button.html',
})
export class CustomButton implements CustomButtonComponent {
  readonly label = input.required<string>();
  readonly disabled = input<boolean>();
  readonly clicked = output();
}
