import { Component, computed, input, OnInit, signal } from '@angular/core';
import { widthToCss } from '../../utils/width-utils';
import { FormGroup } from '@angular/forms';
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
})
export class DynamicField implements OnInit {
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
  readonly FieldType = FieldType;
  readonly field = input.required<FieldDefinition>();
  readonly modelFormGroup = input.required<FormGroup>();
  readonly width = computed(() => widthToCss(getMetadata(this.field(), MetadataType.Width)));

  visible = signal(true);
  label = signal('');
}
