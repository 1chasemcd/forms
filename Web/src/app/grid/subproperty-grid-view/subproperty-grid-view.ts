import { Component, computed, inject, input, OnInit } from '@angular/core';
import { Grid } from '@angular/aria/grid';
import {
  ControlContainer,
  FormArray,
  FormGroup,
  FormGroupDirective,
  ReactiveFormsModule,
} from '@angular/forms';
import { applyPropertyOrConstant, getMetadata } from '../../utils/api-utils';
import { GridCell } from '../grid-cell/grid-cell';
import { MetadataType, SubPropertyGridViewDefinition } from '../../api/api.g';

@Component({
  selector: 'app-subproperty-grid-view',
  imports: [Grid, ReactiveFormsModule, GridCell],
  templateUrl: './subproperty-grid-view.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class SubpropertyGridViewComponent implements OnInit {
  readonly gridView = input.required<SubPropertyGridViewDefinition>();
  readonly labels: string[] = [];
  private readonly parentForm = inject(ControlContainer) as FormGroupDirective;

  readonly formArray = computed(
    () => this.parentForm.control.get(this.gridView().SubPropertyName) as FormArray<FormGroup>,
  );

  ngOnInit() {
    for (const field of this.gridView().Fields) {
      const index = this.labels.push('') - 1;
      applyPropertyOrConstant(
        getMetadata(field, MetadataType.Label),
        this.parentForm.control,
        (label: string) => (this.labels[index] = label),
      );
    }
  }
}
