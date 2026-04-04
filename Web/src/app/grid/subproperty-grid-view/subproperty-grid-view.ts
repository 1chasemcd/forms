import { Component, computed, input, OnInit } from '@angular/core';
import { Grid } from '@angular/aria/grid';
import { FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { applyPropertyOrConstant, getLabel, getMetadata } from '../../utils/api-utils';
import { GridCell } from '../grid-cell/grid-cell';
import { FieldDefinition, MetadataType, SubPropertyGridViewDefinition } from '../../api/api.g';

@Component({
  selector: 'app-subproperty-grid-view',
  imports: [Grid, ReactiveFormsModule, GridCell],
  templateUrl: './subproperty-grid-view.html',
  host: {
    class: 'col-span-12',
  },
})
export class SubpropertyGridViewComponent implements OnInit {
  readonly gridView = input.required<SubPropertyGridViewDefinition>();
  readonly modelFormGroup = input.required<FormGroup>();
  readonly labels: string[] = [];

  readonly formArray = computed(
    () => this.modelFormGroup().get(this.gridView().subPropertyName) as FormArray<FormGroup>,
  );

  ngOnInit() {
    for (const field of this.gridView().fields) {
      const index = this.labels.push('') - 1;
      applyPropertyOrConstant(
        getLabel(field),
        this.modelFormGroup(),
        (label: string) => (this.labels[index] = label),
      );
    }
  }

  readonly gridTemplateColumns = computed(() => {
    return this.columnSpans()
      .map((span) => `minmax(calc(100% / 12 * ${span}), 1fr)`)
      .join(' ');
  });

  private readonly columnSpans = computed(() => {
    const columns = this.gridView().fields;

    const explicit = columns.map((c) => this.getFieldWidth(c));
    const definedTotal = explicit.reduce((sum, w) => (sum ?? 0) + (w ?? 0), 0) ?? 0;

    const undefinedCount = explicit.filter((w) => w == null).length;

    const remaining = Math.max(12 - definedTotal, 0);

    const autoWidth = undefinedCount > 0 ? remaining / undefinedCount : 0;

    return columns.map((c, i) => {
      const w = explicit[i];
      return w ?? autoWidth;
    });
  });

  getFieldWidth(field: FieldDefinition) {
    return getMetadata<number>(field, MetadataType.Width);
  }
}
