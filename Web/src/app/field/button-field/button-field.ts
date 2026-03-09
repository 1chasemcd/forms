import { Component, computed, input } from '@angular/core';
import { ButtonInput } from '../../api/api.g';
import { computedPropertyOrConstant, FormModel } from '../../utils/api-model-utils';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-button-field',
  imports: [],
  templateUrl: './button-field.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class ButtonField {
  readonly button = input<ButtonInput>();
  readonly model = input<FormModel>();

  readonly label = computedPropertyOrConstant<string>(
    computed(() => this.button()?.Label),
    this.model,
  );

  onClick() {
    console.log('Button clicked, perform action');
  }
}
