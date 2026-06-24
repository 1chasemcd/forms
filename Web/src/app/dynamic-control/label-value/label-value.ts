import { Component, forwardRef, input, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-label-value',
  templateUrl: './label-value.html',
  styleUrl: './label-value.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => LabelValue),
      multi: true,
    },
  ],
})
export class LabelValue implements ControlValueAccessor {
  readonly value = signal('');
  writeValue(value: string): void {
    this.value.set(value);
  }
  registerOnChange(_: (_: string) => void) {
    return;
  }
  registerOnTouched(_: () => void) {
    return;
  }
  readonly label = input.required<string>();
}
