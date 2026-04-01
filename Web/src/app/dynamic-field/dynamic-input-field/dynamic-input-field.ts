import { Component, computed, inject, input, OnInit, signal, Type } from '@angular/core';
import {
  ControlContainer,
  FormControl,
  FormGroupDirective,
  ReactiveFormsModule,
} from '@angular/forms';
import { CUSTOM_FIELDS } from '../../field-resolution/custom-field-provider';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
import { NgComponentOutlet } from '@angular/common';
import { RecalculateEventService } from '../../recalculate-event-service/recalculate-event-service';
import { applyPropertyOrConstant, getMetadata } from '../../utils/api-utils';
import { FieldDefinition, MetadataType, RecalculateEvent } from '../../api/api.g';

@Component({
  selector: 'app-dynamic-input-field',
  imports: [ReactiveFormsModule, NgComponentOutlet],
  template: `
    @if (inputComponent(); as component) {
      <ng-container
        (focusout)="onFocusOut()"
        *ngComponentOutlet="component; inputs: { label: label(), formControl: control() }"
      >
      </ng-container>
    }
  `,
  host: {
    '(focusout)': 'onFocusOut()',
  },
  providers: [RecalculateEventService],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicInputField implements OnInit {
  readonly field = input.required<FieldDefinition>();
  private parentForm = inject(ControlContainer) as FormGroupDirective;
  private registry = inject(CUSTOM_FIELDS);
  private recalculateEventService = inject(RecalculateEventService);

  readonly label = signal('');
  readonly control = computed(
    () => this.parentForm.control.get(this.field().Property) as FormControl,
  );

  inputComponent = computed(() => {
    return this.registry.find((r) => r.type === this.field().Type)
      ?.component as Type<CustomInputComponent>;
  });

  onFocusOut() {
    const recalc = getMetadata<RecalculateEvent>(this.field(), MetadataType.RecalculateEvent);
    if (recalc) this.recalculateEventService.runRecalculate(this.parentForm.control, recalc);
  }

  ngOnInit(): void {
    const i = this.field();
    applyPropertyOrConstant(
      getMetadata(i, MetadataType.Label),
      this.parentForm.control,
      this.label.set,
    );
  }
}
