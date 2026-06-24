import { Component, computed, inject, input, OnInit, signal, WritableSignal } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GridCell } from '../grid-cell/grid-cell';
import { GridSelectionType, SubPropertyGridView } from '../../api/api.g';
import { CheckboxInput } from '../../dynamic-control/checkbox/checkbox-input';
import { ControlPath, joinPath } from '../../utils/form-utils';
import { pascalCaseToWords } from '../../utils/string-utils';
import { Icon } from '../../components/icon/icon';
import { FormStackService } from '../../form/form-services/form-stack-service';

@Component({
  selector: 'app-subproperty-grid-view',
  imports: [ReactiveFormsModule, GridCell, CheckboxInput, Icon],
  templateUrl: './subproperty-grid-view.html',
  host: {
    class: 'col-span-12',
  },
})
export class SubpropertyGridViewComponent implements OnInit {
  readonly GridSelectionType = GridSelectionType;

  readonly view = input.required<SubPropertyGridView>();
  readonly path = input.required<ControlPath>();

  private readonly formStack = inject(FormStackService);

  readonly arrayPath = computed(() => joinPath(this.path(), this.view().subProperty));
  readonly labels: WritableSignal<string>[] = [];
  readonly rows = signal<FormGroup[]>([]);

  readonly selectAllControl = new FormControl(false);

  ngOnInit() {
    for (const controlInfo of this.view().controls) {
      const index = this.labels.push(signal(pascalCaseToWords(controlInfo.propertyName))) - 1;
      const controlPath = joinPath(this.arrayPath(), controlInfo.propertyName);
      this.formStack.activeModel.valueRefAugmentor
        .getMetadataValue<string>(controlPath, 'label')
        ?.subscribe((l) => this.labels[index].set(l));
    }
    this.formStack.activeModel
      .get<FormArray>(this.arrayPath())
      ?.valueChanges.subscribe((x: FormArray) => this.rows.set(x.controls as FormGroup[]));
  }

  readonly gridTemplateColumns = computed(() => {
    let columns = this.columnSpans()
      .map((span) => `minmax(max-content, ${span}fr)`)
      .join(' ');

    if (this.view().gridSelectionOptions) columns = 'max-content ' + columns;
    if (this.view().editViewId !== undefined) columns += ' max-content';

    return columns;
  });

  private readonly columnSpans = computed(() => {
    const columns = this.view().controls;

    const explicit = columns.map((c) => c.width);
    const definedTotal = explicit.reduce((sum, w) => (sum ?? 0) + (w ?? 0), 0) ?? 0;

    const undefinedCount = explicit.filter((w) => w == undefined).length;

    const remaining = Math.max(12 - definedTotal, 0);

    let autoWidth = undefinedCount > 0 ? remaining / undefinedCount : 0;
    autoWidth = Math.max(autoWidth, 1);

    return columns.map((c, i) => {
      const w = explicit[i];
      return w ?? autoWidth;
    });
  });

  getRowId(row: FormGroup) {
    return row.get(this.view().idProperty)?.value;
  }

  private getRowIndex(row: FormGroup) {
    const id = this.getRowId(row);
    return this.rows().findIndex((x) => this.getRowId(x) == id);
  }

  createRowPath(row: FormGroup) {
    return joinPath(this.arrayPath(), this.getRowIndex(row));
  }

  getControlFromRow(row: FormGroup, propertyName: string) {
    return row.get(propertyName) as FormControl;
  }

  selectionPropertyUpdated(row: FormGroup) {
    if (this.view().gridSelectionOptions?.selectionType == GridSelectionType.Single) {
      if (!this.getSelectionControl(row).value) return;
      this.unselectAllOthers(this.getRowId(row));
    } else if (this.view().gridSelectionOptions?.selectionType == GridSelectionType.Multiple) {
      this.updateSelectAllState();
    }
  }

  selectAllUpdated() {
    const value = this.selectAllControl.value;
    for (const row of this.rows()) {
      this.getSelectionControl(row).setValue(value);
    }
  }

  startEdit(row: FormGroup) {
    const viewId = this.view().editViewId;
    if (!viewId) return;
    this.formStack.pushSubproperty(viewId, this.createRowPath(row));
  }

  private unselectAllOthers(idToKeep: unknown) {
    for (const row of this.rows()) {
      if (this.getRowId(row) === idToKeep) continue;
      this.getSelectionControl(row).setValue(false);
    }
  }

  private updateSelectAllState() {
    let allSelected = true;
    for (const row of this.rows()) {
      const value = this.getSelectionControl(row).value;
      if (!value) allSelected = false;
    }
    if (allSelected) this.selectAllControl.setValue(true);
    else this.selectAllControl.setValue(false);
  }

  private getSelectionControl(row: FormGroup) {
    const selectionProperty = this.view().gridSelectionOptions?.selectionProperty ?? '';
    return this.getControlFromRow(row, selectionProperty);
  }
}
