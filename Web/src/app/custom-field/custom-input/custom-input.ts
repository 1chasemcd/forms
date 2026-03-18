import { NgClass } from '@angular/common';
import { Component, computed, forwardRef, input, OnInit } from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';

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
export class CustomInput<T> implements ControlValueAccessor, OnInit {
  readonly label = input.required<string>();
  readonly isRequired = input<boolean>();
  readonly textAlign = input<'left' | 'right'>();
  readonly transformDisplayOnChange = input<(value: string) => string>();
  readonly transformDisplayOnFocus = input<(value: string) => string>();
  readonly transformDisplayOnBlur = input<(value: string) => string>();

  readonly controlToDisplay = input<(value: T) => string>();
  readonly displayToControl = input<(value: string) => T>();

  readonly displayControl = new FormControl<string>('', { nonNullable: true });

  readonly requiredMark = computed(() => (this.isRequired() ? '*' : ''));

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  private _onChange = (_: T) => {};
  private _onTouched = () => {};

  private static _nextId = 0;
  readonly id = `input-${CustomInput._nextId++}`;

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

  handleInput(value: string) {
    let transformed = value;
    const changeTransform = this.transformDisplayOnChange();
    if (changeTransform) {
      transformed = changeTransform(value);
      this.displayControl.setValue(transformed, { emitEvent: false });
    }

    const displayToControl = this.displayToControl();
    const controlValue = displayToControl
      ? displayToControl(transformed)
      : (transformed as unknown as T);

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

  ngOnInit() {
    this.displayControl.valueChanges.subscribe((value) => this.handleInput(value));
  }
}
