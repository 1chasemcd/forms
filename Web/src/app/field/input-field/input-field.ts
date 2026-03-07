import { Component, computed, input } from '@angular/core';
import { BaseInput } from '../../api/api.g';
import { computedPropertyOrConstant, FormModel } from '../../utils/api-model-utils';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-input-field',
  imports: [ReactiveFormsModule],
  templateUrl: './input-field.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class InputField {
  readonly baseInput = input<BaseInput>();
  readonly model = input<FormModel>();

  readonly label = computedPropertyOrConstant(
    computed(() => this.baseInput()?.Label),
    this.model,
  );

  readonly inputType = computed(() => {
    switch (this.baseInput()?.$type) {
      case 'checkboxinput':
        return 'checkbox';
      case 'currencyinput':
      case 'numericinput':
        return 'number';
      case 'dateinput':
        return 'date';
      case 'timeinput':
        return 'time';
      case 'textareainput':
      case 'textinput':
      default:
        return 'text';
    }
  });
}
