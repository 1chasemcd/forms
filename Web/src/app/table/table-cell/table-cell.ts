import {
  ChangeDetectorRef,
  Component,
  computed,
  inject,
  input,
  OnInit,
  signal,
  viewChild,
} from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { FieldType, FormFieldInfoContainer, SubPropertyTableView } from '../../api/api.g';
import { MetadataLookupService } from '../../metadata/metadata-lookup';
import { ControlPath, joinPath, parentPath } from '../../utils/form-utils';
import { TableCellContent } from './table-cell-content';
import { ServiceMethodService } from '../../service-method/service-method-service';
import { DynamicInput } from '../../dynamic-control/dynamic-input/dynamic-input';
import { startWith } from 'rxjs';
import { FormStackService } from '../../form/form-services/form-stack-service';

@Component({
  selector: 'app-table-cell',
  imports: [ReactiveFormsModule, TableCellContent, DynamicInput],
  templateUrl: './table-cell.html',
})
export class TableCell implements OnInit {
  readonly ControlType = FieldType;

  readonly fieldInfo = input.required<FormFieldInfoContainer>();
  readonly rowModelPath = input.required<ControlPath>();
  readonly parentView = input.required<SubPropertyTableView>();

  private readonly metadataLookup = inject(MetadataLookupService);
  private readonly formStack = inject(FormStackService);
  private readonly cdr = inject(ChangeDetectorRef);

  private readonly serviceMethodService = inject(ServiceMethodService);

  private readonly inputElement = viewChild(DynamicInput);

  readonly draft = signal<FormControl | null>(null);

  private readonly path = computed(() =>
    joinPath(this.rowModelPath(), this.fieldInfo().identifier),
  );
  readonly control = computed(() => this.formStack.activeModel.get<FormControl>(this.path()));
  readonly fieldType = computed(
    () =>
      this.formStack.activeModel.valueRefAugmentor.getMetadataValue(this.path(), 'fieldType') ??
      FieldType.Text,
  );

  readonly controlEnabled = signal(false);
  readonly rowEnabled = signal(true);
  readonly tableEnabled = signal(false);
  readonly isEnabled = computed(
    () => this.controlEnabled() && this.rowEnabled() && this.tableEnabled(),
  );

  ngOnInit() {
    this.setupEnablement();
  }

  private setupEnablement() {
    const parentView = this.parentView();
    if (parentView.editViewId !== undefined) return;
    if (parentView.canEdit)
      this.formStack.activeModel.valueRefAugmentor
        .getValue(parentPath(parentPath(this.rowModelPath())), parentView.canEdit)
        ?.subscribe((x) => this.tableEnabled.set(!!x));
    if (parentView.canEdit?.$type === 'constant' && !parentView.canEdit.value) return;
    if (parentView.canEditRow)
      this.formStack.activeModel.valueRefAugmentor
        .getValue(this.rowModelPath(), parentView.canEditRow)
        ?.subscribe((x) => this.rowEnabled.set(!!x));
    if (parentView.canEditRow?.$type === 'constant' && !parentView.canEditRow.value) return;
    this.control()
      ?.statusChanges.pipe(startWith(this.control()?.enabled))
      .subscribe(() => this.controlEnabled.set(this.control()?.enabled ?? false));
  }

  startEdit(event: FocusEvent) {
    if (!this.isEnabled()) return;
    if (this.draft()) return;
    const tempControl = new FormControl(
      this.control()?.value,
      this.control()?.validator,
      this.control()?.asyncValidator,
    );
    event?.preventDefault();
    this.draft.set(tempControl);
    this.cdr.detectChanges();
    queueMicrotask(() => {
      this.inputElement()?.focus();
    });
  }

  stopEdit() {
    this.commitValue();
    setTimeout(() => this.draft.set(null));

    this.executeServiceMethod();
  }

  private commitValue() {
    const control = this.control();
    control?.setValue(this.draft()?.value);
    control?.markAsDirty();
  }

  maybeHandleCheckbox() {
    if (!this.isEnabled()) return;
    if (this.draft()) return;
    if (this.fieldType() !== FieldType.CheckBox) return;
    const current = this.control()?.value;
    this.control()?.setValue(!current);
    this.executeServiceMethod();
  }

  executeServiceMethod() {
    const method = this.formStack.activeModel.valueRefAugmentor.getMetadataValue(
      this.path(),
      'formServiceMethod',
    );
    if (method) this.serviceMethodService.runMethod(this.rowModelPath(), method);
  }
}
