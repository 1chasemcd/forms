import { Component, computed, input } from '@angular/core';
import { BaseInput } from '../../api/api.g';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-grid-cell',
  imports: [ReactiveFormsModule],
  templateUrl: './grid-cell.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class GridCell {
  readonly field = input.required<BaseInput>();
  readonly inputType = computed(() => {
    switch (this.field().$type) {
      case 'checkboxinput':
        return 'checkbox';
      case 'currencyinput':
      case 'numericinput':
        return 'numeric';
      case 'dateinput':
        return 'date';
      case 'textinput':
      case 'textareainput':
        return 'text';
      case 'timeinput':
        return 'time';
    }
  });
}
