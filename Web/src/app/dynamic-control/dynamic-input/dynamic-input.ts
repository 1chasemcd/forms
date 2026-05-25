import { Component, input, output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { StandardInputDirective } from '../standard-input/standard-input-directive';
import { NumberFormatDirective } from '../../formatters/number-format-directive';
import { ControlType } from '../../api/api.g';

@Component({
  selector: 'app-dynamic-input',
  imports: [ReactiveFormsModule, StandardInputDirective, NumberFormatDirective],
  templateUrl: './dynamic-input.html',
  host: {
    '(focusin)': 'focused()',
    '(focusout)': 'blurred()',
  },
})
export class DynamicInput {
  readonly ControlType = ControlType;
  readonly controlType = input.required<ControlType>();
  readonly control = input.required<FormControl>();
  readonly valueChange = output();
  private initialValue = '';

  focused() {
    this.initialValue = this.control().value;
  }

  blurred() {
    if (this.initialValue === this.control().value) return;
    this.valueChange.emit();
  }
}
