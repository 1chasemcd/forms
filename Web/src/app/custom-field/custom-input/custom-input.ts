import { NgClass } from '@angular/common';
import { Component, computed, forwardRef, input, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-custom-input',
  imports: [ReactiveFormsModule, NgClass],
  templateUrl: './custom-input.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomInput),
      multi: true,
    },
  ],
})
export class CustomInput<T> implements ControlValueAccessor {
  readonly label = input.required<string>();
  readonly isRequired = input<boolean>();
  readonly textAlign = input<'left' | 'right'>();
  readonly transformDisplayOnChange = input<(value: string) => string>();
  readonly transformDisplayOnFocus = input<(value: string) => string>();
  readonly transformDisplayOnBlur = input<(value: string) => string>();

  readonly controlToDisplay = input<(value: T) => string>();
  readonly displayToControl = input<(value: string) => T>();

  readonly displayValue = signal<string>('');
  readonly isDisabled = signal<boolean>(false);

  readonly requiredMark = computed(() => (this.isRequired() ? '*' : ''));

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  private _onChange = (_: T) => {};
  private _onTouched = () => {};

  private static _nextId = 0;
  readonly id = `input-${CustomInput._nextId++}`;

  writeValue(value: T): void {
    const transform = this.controlToDisplay();
    if (transform) this.displayValue.set(transform(value));
    else this.displayValue.set(value as string);
  }
  registerOnChange(fn: (value: unknown) => void): void {
    this._onChange = fn;
  }
  registerOnTouched(fn: () => void): void {
    this._onTouched = fn;
  }
  setDisabledState(isDisabled: boolean): void {
    this.isDisabled.set(isDisabled);
  }

  handleInput(event: Event) {
    const value = (event.target as HTMLInputElement).value;

    const transform = this.transformDisplayOnChange();
    if (transform) this.displayValue.set(transform(value));

    const displayToControl = this.displayToControl();
    if (displayToControl) this._onChange(displayToControl(this.displayValue()));
    else this._onChange(value as T);
  }

  handleFocus() {
    const transform = this.transformDisplayOnFocus();
    if (transform) this.displayValue.set(transform(this.displayValue()));
  }

  handleBlur() {
    const transform = this.transformDisplayOnBlur();
    if (transform) this.displayValue.set(transform(this.displayValue()));
    this._onTouched();
  }
}
