import { Component, computed, inject, input, OnInit, output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormFieldInfoContainer, TableSelectionType, SubPropertyTableView } from '../../api/api.g';
import { pascalCaseToWords } from '../../utils/string-utils';
import { FormStackService } from '../../form/form-services/form-stack-service';
import { MatTableModule } from '@angular/material/table';
import { DataSource } from '@angular/cdk/table';
import { MetadataLookupService } from '../../metadata/metadata-lookup';
import { MatCheckbox } from '@angular/material/checkbox';
import { Observable } from 'rxjs';
import { SelectionChange, SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-base-table',
  imports: [MatTableModule, ReactiveFormsModule, MatCheckbox],
  templateUrl: './base-table.html',
})
export class BaseTable implements OnInit {
  readonly TableSelectionType = TableSelectionType;
  readonly SELECTION = '$SELECTION';

  readonly dataSource = input.required<DataSource<FormGroup>>();
  readonly view = input.required<SubPropertyTableView>();
  readonly modelType = input.required<string>();
  readonly selectionChangeToModel = output<SelectionChange<unknown>>();
  readonly modelToSelection = input<Observable<unknown[]>>();

  private readonly formStack = inject(FormStackService);
  private readonly metadata = inject(MetadataLookupService);
  readonly columns = computed(() => this.view().fields.map((x) => x.identifier));
  get selection() {
    return this._selection;
  }
  private _selection: SelectionModel<unknown> = new SelectionModel();

  ngOnInit(): void {
    this.setupSelection();
  }

  private setupSelection() {
    const selectionOptions = this.view().tableSelectionOptions;
    if (!selectionOptions) return;
    this._selection = new SelectionModel<unknown>(
      selectionOptions.selectionType === TableSelectionType.Multiple,
    );
    this.modelToSelection()?.subscribe((x) => {
      this.selection.clear(false);
      this.selection.select(x);
    });
    this.selection.changed.subscribe((x) => this.selectionChangeToModel.emit(x));
  }

  getLabel(column: FormFieldInfoContainer) {
    // TODO how do we do path?
    const label = this.metadata.getPropertyMetadata(this.modelType(), column.identifier, 'label');
    if (label?.$type === 'constant') return label.value as string;
    return pascalCaseToWords(column.identifier);
  }
}
