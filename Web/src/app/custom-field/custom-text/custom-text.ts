import { Component, input } from '@angular/core';
import { CustomStaticTextComponent } from '../../field-resolution/custom-field-registration';

@Component({
  selector: 'app-custom-text',
  imports: [],
  templateUrl: './custom-text.html',
})
export class CustomText implements CustomStaticTextComponent {
  readonly label = input.required<string>();
}
