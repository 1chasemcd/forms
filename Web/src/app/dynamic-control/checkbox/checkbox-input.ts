import { Component, ElementRef, input, output, viewChild } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CheckboxIcon } from './checkbox-icon';

@Component({
  selector: 'app-checkbox-input',
  imports: [ReactiveFormsModule, CheckboxIcon],
  templateUrl: './checkbox-input.html',
  host: {
    class: 'max-h-full focus-within:border-violet-600 rounded border border-transparent p-px',
  },
})
export class CheckboxInput {
  readonly control = input.required<FormControl>();
  readonly checkedChange = output();
  private readonly inputEl = viewChild<ElementRef<HTMLInputElement>>('input');

  focus() {
    const el = this.inputEl()?.nativeElement;
    el?.focus();
  }
}
