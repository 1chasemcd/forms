import { Component, computed, input } from '@angular/core';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';
import { FieldDefinition, FieldType } from '../../api/api.g';

@Component({
  selector: 'app-grid-cell',
  imports: [ReactiveFormsModule],
  templateUrl: './grid-cell.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class GridCell {
  readonly field = input.required<FieldDefinition>();
  readonly inputType = computed(() => {
    switch (this.field().Type) {
      case FieldType.CheckBox:
        return 'checkbox';
      case FieldType.Currency:
      case FieldType.Numeric:
        return 'numeric';
      case FieldType.Date:
        return 'date';
      case FieldType.Text:
      case FieldType.TextArea:
      case FieldType.LabelValue:
        return 'text';
      case FieldType.Time:
        return 'time';
      case FieldType.Button:
        return 'button';
    }
  });
}
