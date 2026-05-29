import { Component, computed, input } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ControlType } from '../../api/api.g';
import { CheckboxIcon } from '../../dynamic-control/checkbox/checkbox-icon';
import { CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-grid-cell-content',
  imports: [ReactiveFormsModule, CheckboxIcon, DatePipe, CurrencyPipe, DecimalPipe],
  templateUrl: './grid-cell-content.html',
})
export class GridCellContent {
  readonly ControlType = ControlType;

  readonly value = input.required<string | number | boolean>();
  readonly controlType = input.required<ControlType>();

  readonly valueAsString = computed(() => this.value() as string);
  readonly valueAsNumber = computed(() => this.value() as number);
  readonly valueAsTime = computed(() => new Date(`1970-01-01T${this.value()}`));
  readonly shouldFloatRight = computed(
    () => this.controlType() === ControlType.Numeric || this.controlType() === ControlType.Currency,
  );
}
