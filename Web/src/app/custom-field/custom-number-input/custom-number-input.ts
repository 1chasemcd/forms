// import { Component, ElementRef, input, OnInit, signal, viewChild } from '@angular/core';
// import { FormControl, ReactiveFormsModule } from '@angular/forms';
// import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
// import { CustomInputContainer } from '../custom-input-container/custom-input-container';
// import { CustomInputDirective } from '../../dynamic-field/standard-input/standard-input-directive';

// @Component({
//   selector: 'app-custom-number-input',
//   imports: [ReactiveFormsModule, CustomInputContainer, CustomInputDirective],
//   template: `<app-custom-input-container [label]="label()">
//     <input
//       #input
//       type="text"
//       (input)="onInput()"
//       [required]="required()"
//       [disabled]="disabled()"
//       class="text-right"
//       appCustomInputDirective
//     />
//   </app-custom-input-container>`,
// })
// export class CustomNumberInput implements CustomInputComponent, OnInit {
//   readonly label = input.required<string>();
//   readonly formControl = input.required<FormControl>();
//   readonly required = signal(false);
//   readonly disabled = signal(false);
//   readonly input = viewChild.required<ElementRef<HTMLInputElement>>('input');

//   onInput() {
//     const input = this.input().nativeElement;
//     const rawValue = input.value;
//     const cursor = input.selectionStart ?? rawValue.length;

//     const controlValue = this.stringToNumber(rawValue);
//     const newValue = this.formatNumber(rawValue);
//     this.formControl().setValue(controlValue);
//     input.value = newValue;

//     const newCursor = this.getNewCursorPosition(cursor, rawValue, newValue);
//     requestAnimationFrame(() => input.setSelectionRange(newCursor, newCursor));
//   }

//   formatNumber(value: number | string | null): string {
//     if (value === null || value === undefined) return '';
//     const asNumber = typeof value === 'number' ? value : (this.stringToNumber(value) ?? 0);
//     let formatted = new Intl.NumberFormat('en-US', { maximumFractionDigits: 20 }).format(asNumber);
//     if (typeof value === 'string') {
//       const match = value.match(/\.0*$/);
//       if (match) formatted += match[0];
//       if (value.includes('-') && !formatted.includes('-')) formatted = '-' + formatted;
//     }
//     return formatted;
//   }

//   stringToNumber(value: string): number | null {
//     if (!value) return null;

//     let numeric = value.replace(/[^\d\\.-]/g, '');
//     numeric = numeric.replace(/(?!^)-/g, '');
//     numeric = numeric.replace(/\.(?=.*\.)/g, '');
//     const parsed = Number(numeric);

//     return isNaN(parsed) ? null : parsed;
//   }

//   private getNewCursorPosition(
//     originalCursorPosition: number,
//     originalValue: string,
//     newValue: string,
//   ): number {
//     return originalCursorPosition + (newValue.length - originalValue.length);
//   }

//   ngOnInit() {
//     this.formControl().valueChanges.subscribe(
//       (v) => (this.input().nativeElement.value = this.formatNumber(v)),
//     );
//   }
// }
