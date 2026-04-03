// import { Component, computed, inject, input } from '@angular/core';
// import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
// import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
// import { NgComponentOutlet } from '@angular/common';
// import { RecalculateEventService } from '../../recalculate-event-service/recalculate-event-service';
// import { getMetadata } from '../../utils/api-utils';
// import { FieldDefinition, MetadataType, RecalculateEvent } from '../../api/api.g';
// import { CustomFieldService } from '../../field-resolution/custom-field-service';

// @Component({
//   selector: 'app-dynamic-input-field',
//   imports: [ReactiveFormsModule, NgComponentOutlet],
//   template: `
//     @if (inputComponent(); as component) {
//       <ng-container
//         *ngComponentOutlet="component; inputs: { label: label(), formControl: control() }"
//       >
//       </ng-container>
//     }
//   `,
//   host: {
//     '(focusin)': 'onFocusIn()',
//     '(focusout)': 'onFocusOut()',
//   },
//   providers: [RecalculateEventService],
// })
// export class DynamicInputField {
//   readonly label = input.required<string>();
//   readonly field = input.required<FieldDefinition>();
//   readonly modelFormGroup = input.required<FormGroup>();
//   private originalValue: unknown;

//   private readonly customFieldService = inject(CustomFieldService);
//   private readonly recalculateEventService = inject(RecalculateEventService);

//   readonly control = computed(
//     () => this.modelFormGroup().get(this.field().Property) as FormControl,
//   );

//   inputComponent = computed(() => {
//     return this.customFieldService.getField<CustomInputComponent>(this.field().Type);
//   });

//   onFocusIn() {
//     this.originalValue = this.control().value;
//   }
//   onFocusOut() {
//     if (this.originalValue === this.control().value) return;
//     const recalc = getMetadata<RecalculateEvent>(this.field(), MetadataType.RecalculateEvent);
//     if (recalc) this.recalculateEventService.runRecalculate(this.modelFormGroup(), recalc);
//   }
// }
