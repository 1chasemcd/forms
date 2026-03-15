import { Component, input } from '@angular/core';
import { CustomStaticTextComponent } from '../../field-resolution/custom-field-registration';

@Component({
  selector: 'app-custom-static-text',
  imports: [],
  templateUrl: './custom-static-text.html',
})
export class CustomStaticText implements CustomStaticTextComponent {
  readonly label = input.required<string>();
}
