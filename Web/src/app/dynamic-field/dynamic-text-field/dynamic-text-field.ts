import { NgComponentOutlet } from '@angular/common';
import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { StaticTextField } from '../../api/api.g';
import { CUSTOM_FIELDS } from '../../field-resolution/custom-field-provider';
import { FormModel } from '../../dynamic-form/form-model';

@Component({
  selector: 'app-dynamic-text-field',
  imports: [NgComponentOutlet],
  template: `
    @if (staticTextComponent(); as component) {
      <ng-container *ngComponentOutlet="component; inputs: { label: label() }"></ng-container>
    }
  `,
})
export class DynamicTextField implements OnInit {
  readonly staticText = input.required<StaticTextField>();
  readonly model = input.required<FormModel>();
  readonly label = signal('');
  private registry = inject(CUSTOM_FIELDS);

  staticTextComponent = computed(() => {
    return this.registry.find((r) => r.type === 'statictextfield')?.component;
  });

  ngOnInit(): void {
    this.model().registerPocDependency(this.staticText().Label, this.label.set);
  }
}
