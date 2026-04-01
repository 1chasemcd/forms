import { NgComponentOutlet } from '@angular/common';
import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { CUSTOM_FIELDS } from '../../field-resolution/custom-field-provider';
import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { applyPropertyOrConstant, getMetadata } from '../../utils/api-utils';
import { FieldDefinition, FieldType, MetadataType } from '../../api/api.g';

@Component({
  selector: 'app-dynamic-text-field',
  imports: [NgComponentOutlet],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
  template: `
    @if (staticTextComponent(); as component) {
      <ng-container *ngComponentOutlet="component; inputs: { label: label() }"></ng-container>
    }
  `,
})
export class DynamicTextField implements OnInit {
  readonly field = input.required<FieldDefinition>();
  readonly label = signal('');
  private parentForm = inject(ControlContainer) as FormGroupDirective;
  private registry = inject(CUSTOM_FIELDS);

  staticTextComponent = computed(() => {
    return this.registry.find((r) => r.type === FieldType.LabelValue)?.component;
  });

  ngOnInit(): void {
    applyPropertyOrConstant(
      getMetadata(this.field(), MetadataType.Label),
      this.parentForm.control,
      this.label.set,
    );
  }
}
