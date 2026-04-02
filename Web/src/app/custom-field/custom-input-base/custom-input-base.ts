import { NgClass } from '@angular/common';
import { Component, computed, forwardRef, input } from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';

@Component({
  selector: 'app-custom-input-base',
  imports: [ReactiveFormsModule, NgClass],
  templateUrl: './custom-input-base.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomInputBase),
      multi: true,
    },
  ],
})
export class CustomInputBase<T> implements ControlValueAccessor {
  readonly label = input.required<string>();
  readonly isRequired = input<boolean>();
  readonly textAlign = input<'left' | 'right'>();
  readonly inputType = input<string>('text');
  readonly transformDisplayOnChange = input<(value: string) => string>();
  readonly transformDisplayOnFocus = input<(value: string) => string>();
  readonly transformDisplayOnBlur = input<(value: string) => string>();

  readonly controlToDisplay = input<(value: T) => string>();
  readonly displayToControl = input<(value: string) => T>();

  readonly displayControl = new FormControl<string>('', { nonNullable: true });

  readonly requiredMark = computed(() => (this.isRequired() ? ' *' : ''));

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  private _onChange = (_: T) => {};
  private _onTouched = () => {};

  private static _nextId = 0;
  readonly id = `input-${CustomInputBase._nextId++}`;

  private inputInProgress = false;

  writeValue(value: T): void {
    if (this.inputInProgress) return;
    const transform = this.controlToDisplay();
    const display = transform ? transform(value) : (value as unknown as string);
    this.displayControl.setValue(display, { emitEvent: false });
  }
  registerOnChange(fn: (value: unknown) => void): void {
    this._onChange = fn;
  }
  registerOnTouched(fn: () => void): void {
    this._onTouched = fn;
  }
  setDisabledState(isDisabled: boolean): void {
    if (isDisabled) this.displayControl.disable();
    else this.displayControl.enable();
  }

  handleInput(event: Event) {
    const input = event.target as HTMLInputElement;
    const inputType = (event as InputEvent).inputType;

    const rawValue = input.value;
    let newValue = rawValue;
    const cursor = input.selectionStart ?? rawValue.length;

    const changeTransform = this.transformDisplayOnChange();
    if (changeTransform) {
      newValue = changeTransform(rawValue);
      this.displayControl.setValue(newValue, { emitEvent: false });
      const newCursor = this.getNewCursorPosition(cursor, inputType, rawValue, newValue);
      requestAnimationFrame(() => input.setSelectionRange(newCursor, newCursor));
    }

    this.updateControl(newValue);
  }

  private getNewCursorPosition(
    originalCursorPosition: number,
    inputEventType: string,
    originalValue: string,
    newValue: string,
  ): number {
    if (inputEventType === 'insertText' && newValue.length < originalValue.length)
      originalCursorPosition += 1;
    return originalCursorPosition + (newValue.length - originalValue.length);
  }

  private updateControl(newValue: string) {
    const displayToControl = this.displayToControl();
    const controlValue = displayToControl ? displayToControl(newValue) : (newValue as unknown as T);

    this.inputInProgress = true;
    this._onChange(controlValue);
    this.inputInProgress = false;
  }

  handleFocus() {
    const transform = this.transformDisplayOnFocus();
    if (!transform) return;
    this.displayControl.setValue(transform(this.displayControl.value), { emitEvent: false });
  }

  handleBlur() {
    const transform = this.transformDisplayOnBlur();
    if (!transform) return;
    this.displayControl.setValue(transform(this.displayControl.value), { emitEvent: false });
    this._onTouched();
  }
}
