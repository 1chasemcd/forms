import { Component, forwardRef, input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { formatAsDate, mapIndices } from '../../utils/string-utils';

@Component({
  selector: 'app-date-input',
  template: `
    <mat-form-field class="max-w-full">
      <mat-label>{{ label() }}</mat-label>

      <input
        matInput
        [matDatepicker]="picker"
        [value]="dateValue"
        (input)="onInput($event)"
        (dateChange)="onDateChange($event.value)"
        [disabled]="disabled"
      />

      <mat-datepicker-toggle matIconSuffix [for]="picker" [disabled]="disabled" />

      <mat-datepicker #picker />
    </mat-form-field>
  `,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DateInput),
      multi: true,
    },
  ],
  imports: [MatDatepickerModule, MatFormFieldModule, MatInputModule],
})
export class DateInput implements ControlValueAccessor {
  label = input.required<string>();
  dateValue: Date | null = null;
  disabled: boolean = false;

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  private onChange = (_: string | null) => {};
  onTouched = () => {};

  writeValue(value: string | null): void {
    this.dateValue = value ? this.parseDateOnly(value) : null;
  }

  registerOnChange(fn: (value: string | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onInput(event: InputEvent) {
    const target = event.target as HTMLInputElement;
    const initial = target.value;
    let cursorPos = target.selectionStart ?? initial.length;

    const formatted = formatAsDate(initial);

    const indexMap = mapIndices(initial, formatted);
    indexMap.push(formatted.length); // cursor position at end of original should map to end of new string
    let finalPosIndex = cursorPos;
    while (!Number.isInteger(indexMap.at(finalPosIndex)) && finalPosIndex > 0) finalPosIndex--;
    cursorPos = indexMap[finalPosIndex] ?? 0;

    target.value = formatted;
    target.setSelectionRange(cursorPos, cursorPos);
  }

  onDateChange(date: Date | null): void {
    this.dateValue = date;
    this.onChange(date ? this.formatDateOnly(date) : null);
  }

  private parseDateOnly(value: string): Date {
    const [year, month, day] = value.split('-').map(Number);

    return new Date(year, month - 1, day);
  }

  private formatDateOnly(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}
