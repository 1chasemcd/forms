import { Component, computed, inject, input, OnInit, signal, Type } from '@angular/core';
import { BaseInput } from '../../api/api.g';
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
import { applyPropertyOrConstant } from '../../utils/api-utils';

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
  readonly baseInput = input.required<BaseInput>();
  private parentForm = inject(ControlContainer) as FormGroupDirective;
  private registry = inject(CUSTOM_FIELDS);
  private recalculateEventService = inject(RecalculateEventService);

  readonly label = signal('');
  readonly control = computed(
    () => this.parentForm.control.get(this.baseInput().Property) as FormControl,
  );

  inputComponent = computed(() => {
    return this.registry.find((r) => r.type === this.baseInput().$type)
      ?.component as Type<CustomInputComponent>;
  });

  onFocusOut() {
    const recalculate = this.baseInput().RecalculateEvent;
    if (recalculate)
      this.recalculateEventService.runRecalculate(this.parentForm.control, recalculate);
  }

  ngOnInit(): void {
    const i = this.baseInput();
    applyPropertyOrConstant(i.Label, this.parentForm.control, this.label.set);
  }
}
