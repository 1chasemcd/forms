import { Component, input, OnInit, output, signal } from '@angular/core';
import { applyPropertyOrConstant, getMetadata } from '../../utils/api-utils';
import { FormGroup } from '@angular/forms';
import { FieldDefinition, MetadataType } from '../../api/api.g';

@Component({
  selector: 'app-button',
  templateUrl: './button.html',
})
export class Button implements OnInit {
  readonly label = input.required<string>();
  readonly field = input.required<FieldDefinition>();
  readonly modelFormGroup = input.required<FormGroup>();
  readonly recalculateEvent = output();

  readonly enabled = signal(true);

  ngOnInit(): void {
    applyPropertyOrConstant(
      getMetadata(this.field(), MetadataType.Enabled),
      this.modelFormGroup(),
      this.enabled.set,
    );
  }
}
