import { Directive, ElementRef, inject, input, OnInit } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: 'input[appNumberFormat]',
  host: {
    '(input)': 'onInput()',
  },
})
export class NumberFormatDirective implements OnInit {
  readonly maxFractionDigits = input(20);
  private readonly element = inject<ElementRef<HTMLInputElement>>(ElementRef<HTMLInputElement>);
  private readonly control = inject(NgControl, { optional: true });

  ngOnInit() {
    if (this.control?.control) {
      this.control.control.valueChanges.subscribe((v) => {
        this.element.nativeElement.value = this.formatNumber(v);
      });
    }
  }

  onInput() {
    const inputEl = this.element.nativeElement;
    const rawValue = inputEl.value;
    const cursor = inputEl.selectionStart ?? rawValue.length;

    const numberValue = this.stringToNumber(rawValue);
    if (this.control?.control) this.control.control.setValue(numberValue, { emitEvent: false });

    const formatted = this.formatNumber(rawValue);
    inputEl.value = formatted;

    const newCursor = this.getNewCursorPosition(cursor, rawValue, formatted);
    requestAnimationFrame(() => inputEl.setSelectionRange(newCursor, newCursor));
  }

  private formatNumber(value: number | string | null): string {
    if (value == null) return '';
    const asNumber = typeof value === 'number' ? value : (this.stringToNumber(value) ?? 0);

    let formatted = new Intl.NumberFormat('en-US', {
      maximumFractionDigits: this.maxFractionDigits(),
    }).format(asNumber);

    if (typeof value === 'string') {
      const match = value.match(/\.0*$/);
      if (match) formatted += match[0];
      if (value.includes('-') && !formatted.includes('-')) formatted = '-' + formatted;
    }
    return formatted;
  }

  private stringToNumber(value: string): number | null {
    if (!value) return null;
    let numeric = value.replace(/[^\d-\\.]/g, '');
    numeric = numeric.replace(/(?!^)-/g, '');
    numeric = numeric.replace(/\.(?=.*\.)/g, '');
    const parsed = Number(numeric);
    return isNaN(parsed) ? null : parsed;
  }

  private getNewCursorPosition(originalCursor: number, oldValue: string, newValue: string): number {
    return originalCursor + (newValue.length - oldValue.length);
  }
}
