import { Component, computed, input, OnInit } from '@angular/core';
import { Grid } from '@angular/aria/grid';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { applyPropertyOrConstant, getLabel, getMetadata } from '../../utils/api-utils';
import { GridCell } from '../grid-cell/grid-cell';
import {
  FieldDefinition,
  FormDefinition,
  GridSelectionType,
  MetadataType,
  SubPropertyGridViewDefinition,
} from '../../api/api.g';
import { CheckboxInput } from '../../dynamic-field/checkbox/checkbox-input';

@Component({
  selector: 'app-subproperty-grid-view',
  imports: [Grid, ReactiveFormsModule, GridCell, CheckboxInput],
  templateUrl: './subproperty-grid-view.html',
  host: {
    class: 'col-span-12',
  },
})
export class SubpropertyGridViewComponent implements OnInit {
  readonly formDefinition = input.required<FormDefinition>();
  readonly GridSelectionType = GridSelectionType;
  readonly gridView = input.required<SubPropertyGridViewDefinition>();
  readonly modelFormGroup = input.required<FormGroup>();
  readonly labels: string[] = [];

  readonly formArray = computed(
    () => this.modelFormGroup().get(this.gridView().subPropertyName) as FormArray<FormGroup>,
  );

  readonly selectAllControl = new FormControl(false);

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
    let columns = this.columnSpans()
      .map((span) => `minmax(max-content, ${span}fr)`)
      .join(' ');

    if (this.gridView().selectionOptions) columns = 'max-content ' + columns;

    return columns;
  });

  private readonly columnSpans = computed(() => {
    const columns = this.gridView().fields;

    const explicit = columns.map((c) => this.getFieldWidth(c));
    const definedTotal = explicit.reduce((sum, w) => (sum ?? 0) + (w ?? 0), 0) ?? 0;

    const undefinedCount = explicit.filter((w) => w == null).length;

    const remaining = Math.max(12 - definedTotal, 0);

    let autoWidth = undefinedCount > 0 ? remaining / undefinedCount : 0;
    autoWidth = Math.max(autoWidth, 1);

    return columns.map((c, i) => {
      const w = explicit[i];
      return w ?? autoWidth;
    });
  });

  getFieldWidth(field: FieldDefinition) {
    return getMetadata<number>(field, MetadataType.Width);
  }

  getRowId(row: FormGroup) {
    return row.get(this.gridView().idProperty)?.value;
  }

  getControlFromRow(row: FormGroup, propertyName: string) {
    return row.get(propertyName) as FormControl;
  }

  selectionPropertyUpdated(row: FormGroup) {
    if (this.gridView().selectionOptions?.selectionType == GridSelectionType.Single) {
      if (!this.getSelectionControl(row).value) return;
      this.unselectAllOthers(this.getRowId(row));
    } else if (this.gridView().selectionOptions?.selectionType == GridSelectionType.Multiple) {
      this.updateSelectAllState();
    }
  }

  selectAllUpdated() {
    const value = this.selectAllControl.value;
    for (const row of this.formArray().controls) {
      this.getSelectionControl(row).setValue(value);
    }
  }

  private unselectAllOthers(idToKeep: unknown) {
    for (const row of this.formArray().controls) {
      if (this.getRowId(row) === idToKeep) continue;
      this.getSelectionControl(row).setValue(false);
    }
  }

  private updateSelectAllState() {
    let allSelected = true;
    for (const row of this.formArray().controls) {
      const value = this.getSelectionControl(row).value;
      if (!value) allSelected = false;
    }
    if (allSelected) this.selectAllControl.setValue(true);
    else this.selectAllControl.setValue(false);
  }

  private getSelectionControl(row: FormGroup) {
    const selectionProperty = this.gridView().selectionOptions?.selectionProperty ?? '';
    return this.getControlFromRow(row, selectionProperty);
  }
}
