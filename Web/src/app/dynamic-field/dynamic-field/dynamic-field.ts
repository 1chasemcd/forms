import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { widthToCss } from '../../utils/width-utils';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { applyPropertyOrConstant, getMetadata } from '../../utils/api-utils';
import { FieldDefinition, FieldType, MetadataType, RecalculateEvent } from '../../api/api.g';
import { RecalculateEventService } from '../../recalculate-event-service/recalculate-event-service';
import { Button } from '../button/button';
import { Checkbox } from '../checkbox/checkbox';
import { CustomLabelValue } from '../label-value/label-value';
import { DynamicInput } from '../dynamic-input/dynamic-input';
import { StandardInputWrapper } from '../standard-input/standard-input-wrapper';

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
  providers: [RecalculateEventService],
})
export class DynamicField implements OnInit {
  readonly FieldType = FieldType;

  readonly field = input.required<FieldDefinition>();
  readonly modelFormGroup = input.required<FormGroup>();

  readonly width = computed(() => widthToCss(getMetadata(this.field(), MetadataType.Width)));
  readonly control = computed(
    () => this.modelFormGroup().get(this.field().Property) as FormControl,
  );
  readonly visible = signal(true);
  readonly label = signal('');

  private readonly recalculateEventService = inject(RecalculateEventService);

  ngOnInit() {
    applyPropertyOrConstant(
      getMetadata(this.field(), MetadataType.Visible),
      this.modelFormGroup(),
      this.visible.set,
    );

    applyPropertyOrConstant(
      getMetadata(this.field(), MetadataType.Label),
      this.modelFormGroup(),
      this.label.set,
    );
  }

  private initialValue = '';

  onFocus() {
    this.initialValue = this.control().value;
  }

  onBlur() {
    if (this.initialValue === this.control().value) return;
    this.executeRecalculate();
  }

  executeRecalculate() {
    const recalc = getMetadata<RecalculateEvent>(this.field(), MetadataType.RecalculateEvent);
    if (recalc) this.recalculateEventService.runRecalculate(this.modelFormGroup(), recalc);
  }
}
