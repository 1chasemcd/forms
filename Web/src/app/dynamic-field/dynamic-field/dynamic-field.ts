import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { widthToCss } from '../../utils/width-utils';
import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { DynamicInputField } from '../dynamic-input-field/dynamic-input-field';
import { applyPropertyOrConstant, getMetadata } from '../../utils/api-utils';
import { DynamicButtonField } from '../dynamic-button-field/dynamic-button-field';
import { FieldDefinition, FieldType, MetadataType } from '../../api/api.g';
@Component({
  selector: 'app-dynamic-field',
  host: {
    '[class]': 'width() + " content-center"',
  },
  templateUrl: './dynamic-field.html',
  imports: [DynamicInputField, DynamicButtonField],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicField implements OnInit {
  ngOnInit() {
    applyPropertyOrConstant(
      getMetadata(this.field(), MetadataType.Visible),
      this.parentForm.control,
      this.visible.set,
    );
  }
  readonly field = input.required<FieldDefinition>();
  readonly width = computed(() => widthToCss(getMetadata(this.field(), MetadataType.Width)));
  private parentForm = inject(ControlContainer) as FormGroupDirective;

  visible = signal(true);

  readonly buttonField = computed(() => {
    const f = this.field();
    return f.Type === FieldType.Button ? f : null;
  });

  readonly inputField = computed(() => {
    const f = this.field();
    return f.Type !== FieldType.Button ? f : null;
  });
}
