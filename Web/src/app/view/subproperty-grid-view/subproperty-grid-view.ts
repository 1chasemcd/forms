import { Component, computed, inject, input, OnInit } from '@angular/core';
import { Grid } from '@angular/aria/grid';
import { SubPropertyGridView } from '../../api/api.g';
import { FormModel } from '../../dynamic-form/form-model';
import { ControlContainer, FormArray, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-subproperty-grid-view',
  imports: [Grid],
  templateUrl: './subproperty-grid-view.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class SubpropertyGridViewComponent implements OnInit {
  readonly gridView = input.required<SubPropertyGridView>();
  readonly model = input.required<FormModel>();
  readonly labels: string[] = [];
  private readonly parentForm = inject(ControlContainer) as FormGroupDirective;

  readonly formArray = computed(
    () => this.parentForm.control.get(this.gridView().SubPropertyName) as FormArray,
  );

  ngOnInit() {
    for (const field of this.gridView().Fields) {
      const index = this.labels.push('') - 1;
      this.model().registerPocDependency(
        field.Label,
        (label) => (this.labels[index] = label as string),
      );
    }
  }
}
