import { Component, ElementRef, input, output, viewChild } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { StandardInputDirective } from '../standard-input/standard-input-directive';
import { NumberFormatDirective } from '../../formatters/number-format-directive';
import { FieldType } from '../../api/api.g';
import { CheckboxInput } from '../checkbox/checkbox-input';

@Component({
  selector: 'app-dynamic-input',
  imports: [ReactiveFormsModule, StandardInputDirective, NumberFormatDirective, CheckboxInput],
  templateUrl: './dynamic-input.html',
  host: {
    '(focusin)': 'focused()',
    '(focusout)': 'blurred()',
  },
})
export class DynamicInput {
  readonly ControlType = FieldType;
  readonly fieldType = input.required<FieldType>();
  readonly control = input.required<FormControl>();
  readonly valueChange = output();
  private initialValue = '';
  private readonly inputEl = viewChild<ElementRef>('input');

  focus() {
    const el = this.inputEl();
    if (el instanceof HTMLLabelElement) return el.nativeElement.focus();
    const nativeElement = el?.nativeElement;
    nativeElement?.focus();
    if (nativeElement instanceof HTMLInputElement || nativeElement instanceof HTMLTextAreaElement)
      nativeElement.select();
  }

  focused() {
    this.initialValue = this.control().value;
  }

  blurred() {
    if (this.initialValue === this.control().value) return;
    this.valueChange.emit();
  }
}
