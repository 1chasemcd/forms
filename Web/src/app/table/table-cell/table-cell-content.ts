import { Component, computed, input } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { FieldType } from '../../api/api.g';
import { CheckboxIcon } from '../../dynamic-control/checkbox/checkbox-icon';
import { CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-table-cell-content',
  imports: [ReactiveFormsModule, CheckboxIcon, DatePipe, CurrencyPipe, DecimalPipe],
  templateUrl: './table-cell-content.html',
})
export class TableCellContent {
  readonly ControlType = FieldType;

  readonly value = input.required<string | number | boolean>();
  readonly fieldType = input.required<FieldType>();

  readonly valueAsString = computed(() => this.value() as string);
  readonly valueAsNumber = computed(() => this.value() as number);
  readonly valueAsTime = computed(() => new Date(`1970-01-01T${this.value()}`));
  readonly shouldFloatRight = computed(
    () => this.fieldType() === FieldType.Numeric || this.fieldType() === FieldType.Currency,
  );
}
