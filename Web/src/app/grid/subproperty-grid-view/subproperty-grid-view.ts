import { Component, computed, inject, input, OnInit } from '@angular/core';
import { Grid } from '@angular/aria/grid';
import { SubPropertyGridView } from '../../api/api.g';
import {
  ControlContainer,
  FormArray,
  FormGroup,
  FormGroupDirective,
  ReactiveFormsModule,
} from '@angular/forms';
import { applyPropertyOrConstant } from '../../utils/api-utils';
import { GridCell } from '../grid-cell/grid-cell';

@Component({
  selector: 'app-subproperty-grid-view',
  imports: [Grid, ReactiveFormsModule, GridCell],
  templateUrl: './subproperty-grid-view.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class SubpropertyGridViewComponent implements OnInit {
  readonly gridView = input.required<SubPropertyGridView>();
  readonly labels: string[] = [];
  private readonly parentForm = inject(ControlContainer) as FormGroupDirective;

  readonly formArray = computed(
    () => this.parentForm.control.get(this.gridView().SubPropertyName) as FormArray<FormGroup>,
  );

  ngOnInit() {
    for (const field of this.gridView().Fields) {
      const index = this.labels.push('') - 1;
      applyPropertyOrConstant(
        field.Label,
        this.parentForm.control,
        (label: string) => (this.labels[index] = label),
      );
    }
  }
}
