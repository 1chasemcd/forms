import { Component, computed, inject, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import {
  FieldDefinition,
  FieldType,
  FormDefinition,
  MetadataType,
  RecalculateEvent,
} from '../../api/api.g';
import { DynamicInput } from '../../dynamic-field/dynamic-input/dynamic-input';
import { getMetadata } from '../../utils/api-utils';
import { RecalculateEventService } from '../../recalculate-event-service/recalculate-event-service';
import { CheckboxInput } from '../../dynamic-field/checkbox/checkbox-input';
import { FormContext } from '../../dynamic-form/form-context';

@Component({
  selector: 'app-grid-cell',
  imports: [ReactiveFormsModule, DynamicInput, CheckboxInput],
  templateUrl: './grid-cell.html',
})
export class GridCell {
  readonly FieldType = FieldType;

  readonly formDefinition = input.required<FormDefinition>();
  readonly field = input.required<FieldDefinition>();
  readonly rowContext = input.required<FormContext>();

  private readonly recalculateEventService = inject(RecalculateEventService);

  readonly control = computed(
    () => this.rowContext().getOrAddControl(this.field().property) as FormControl,
  );

  executeRecalculate() {
    const recalc = getMetadata<RecalculateEvent>(this.field(), MetadataType.RecalculateEvent);
    if (recalc) this.recalculateEventService.runRecalculate(this.rowContext(), recalc);
  }
}
