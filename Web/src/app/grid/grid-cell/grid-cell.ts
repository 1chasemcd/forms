import { Component, computed, inject, input } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FieldDefinition, FieldType, MetadataType, RecalculateEvent } from '../../api/api.g';
import { DynamicInput } from '../../dynamic-field/dynamic-input/dynamic-input';
import { getMetadata } from '../../utils/api-utils';
import { RecalculateEventService } from '../../recalculate-event-service/recalculate-event-service';
import { CheckboxInput } from '../../dynamic-field/checkbox/checkbox-input';

@Component({
  selector: 'app-grid-cell',
  imports: [ReactiveFormsModule, DynamicInput, CheckboxInput],
  templateUrl: './grid-cell.html',
})
export class GridCell {
  readonly FieldType = FieldType;

  readonly field = input.required<FieldDefinition>();
  readonly rowFormGroup = input.required<FormGroup>();

  private readonly recalculateEventService = inject(RecalculateEventService);

  readonly control = computed(() => this.rowFormGroup().get(this.field().property) as FormControl);

  executeRecalculate() {
    const recalc = getMetadata<RecalculateEvent>(this.field(), MetadataType.RecalculateEvent);
    if (recalc) this.recalculateEventService.runRecalculate(this.rowFormGroup(), recalc);
  }
}
