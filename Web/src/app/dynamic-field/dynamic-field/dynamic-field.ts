import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { widthToCss } from '../../utils/width-utils';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { applyPropertyOrConstant, getLabel, getMetadata } from '../../utils/api-utils';
import { FieldDefinition, FieldType, MetadataType, RecalculateEvent } from '../../api/api.g';
import { RecalculateEventService } from '../../recalculate-event-service/recalculate-event-service';
import { Button } from '../button/button';
import { Checkbox } from '../checkbox/checkbox';
import { CustomLabelValue } from '../label-value/label-value';
import { DynamicInput } from '../dynamic-input/dynamic-input';
import { StandardInputWrapper } from '../standard-input/standard-input-wrapper';
import { FormContext } from '../../dynamic-form/form-context';

@Component({
  selector: 'app-dynamic-field',
  host: {
    '[class]': 'width() + " content-center"',
  },
  templateUrl: './dynamic-field.html',
  imports: [
    Button,
    Checkbox,
    ReactiveFormsModule,
    CustomLabelValue,
    DynamicInput,
    StandardInputWrapper,
  ],
})
export class DynamicField implements OnInit {
  readonly FieldType = FieldType;

  readonly field = input.required<FieldDefinition>();
  readonly formContext = input.required<FormContext>();

  readonly width = computed(() => widthToCss(getMetadata(this.field(), MetadataType.Width)));
  readonly control = computed(
    () => this.formContext().getOrAddControl(this.field().property) as FormControl,
  );
  readonly visible = signal(true);
  readonly label = signal('');

  private readonly recalculateEventService = inject(RecalculateEventService);

  ngOnInit() {
    applyPropertyOrConstant(
      getMetadata(this.field(), MetadataType.Visible),
      this.formContext(),
      this.visible.set,
    );

    applyPropertyOrConstant(getLabel(this.field()), this.formContext(), this.label.set);
  }

  executeRecalculate() {
    const recalc = getMetadata<RecalculateEvent>(this.field(), MetadataType.RecalculateEvent);
    if (recalc) this.recalculateEventService.runRecalculate(this.formContext(), recalc);
  }
}
