import { NgComponentOutlet } from '@angular/common';
import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { StaticTextField } from '../../api/api.g';
import { CUSTOM_FIELDS } from '../../field-resolution/custom-field-provider';
import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { applyPropertyOrConstant } from '../../utils/api-utils';

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
  readonly staticText = input.required<StaticTextField>();
  readonly label = signal('');
  private parentForm = inject(ControlContainer) as FormGroupDirective;
  private registry = inject(CUSTOM_FIELDS);

  staticTextComponent = computed(() => {
    return this.registry.find((r) => r.type === 'statictextfield')?.component;
  });

  ngOnInit(): void {
    applyPropertyOrConstant(this.staticText().Label, this.parentForm.control, this.label.set);
  }
}
