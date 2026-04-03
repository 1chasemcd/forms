import { Component, input, output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { FieldType } from '../../api/api.g';
import { StandardInputWrapper } from '../standard-input/standard-input-wrapper';
import { StandardInputDirective } from '../standard-input/standard-input-directive';
import { NumberFormatDirective } from '../../formatters/number-format-directive';

@Component({
  selector: 'app-dynamic-input',
  imports: [
    ReactiveFormsModule,
    StandardInputWrapper,
    StandardInputDirective,
    NumberFormatDirective,
  ],
  templateUrl: './dynamic-input.html',
})
export class DynamicInput {
  readonly FieldType = FieldType;

  readonly fieldType = input.required<FieldType>();
  readonly control = input.required<FormControl>();
  readonly label = input.required<string>();
  readonly recalculateEvent = output();

  private initialValue = '';

  onFocus() {
    this.initialValue = this.control().value;
  }

  onBlur() {
    if (this.initialValue === this.control().value) return;
    this.recalculateEvent.emit();
  }
}
