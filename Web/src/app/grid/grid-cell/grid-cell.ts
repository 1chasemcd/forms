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
import { ControlType, FormControlInfoContainer, SubPropertyGridView } from '../../api/api.g';
import { MetadataLookupService } from '../../metadata/metadata-lookup-service';
import { ControlPath, joinPath, parentPath } from '../../utils/form-utils';
import { FormModelService } from '../../form/form-services/form-model-service';
import { GridCellContent } from './grid-cell-content';
import { ServiceMethodService } from '../../service-method/service-method-service';
import { DynamicInput } from '../../dynamic-control/dynamic-input/dynamic-input';
import { ControlValueService } from '../../form/form-services/control-value-service';
import { startWith } from 'rxjs';

@Component({
  selector: 'app-grid-cell',
  imports: [ReactiveFormsModule, GridCellContent, DynamicInput],
  templateUrl: './grid-cell.html',
})
export class GridCell implements OnInit {
  readonly ControlType = ControlType;

  readonly controlInfo = input.required<FormControlInfoContainer>();
  readonly rowModelPath = input.required<ControlPath>();
  readonly parentView = input.required<SubPropertyGridView>();

  private readonly metadataLookup = inject(MetadataLookupService);
  private readonly formModelService = inject(FormModelService);
  private readonly controlValues = inject(ControlValueService);
  private readonly cdr = inject(ChangeDetectorRef);

  private readonly serviceMethodService = inject(ServiceMethodService);

  private readonly inputElement = viewChild(DynamicInput);

  readonly draft = signal<FormControl | null>(null);

  private readonly path = computed(() =>
    joinPath(this.rowModelPath(), this.controlInfo().propertyName),
  );
  readonly control = computed(() => this.formModelService.get<FormControl>(this.path()));
  readonly controlType = computed(
    () => this.metadataLookup.getPropertyMetadata(this.path(), 'controlType') ?? ControlType.Text,
  );

  readonly controlEnabled = signal(false);
  readonly rowEnabled = signal(true);
  readonly gridEnabled = signal(false);
  readonly isEnabled = computed(
    () => this.controlEnabled() && this.rowEnabled() && this.gridEnabled(),
  );

  ngOnInit() {
    this.setupEnablement();
  }

  private setupEnablement() {
    const parentView = this.parentView();
    if (parentView.editViewId !== undefined) return;
    if (parentView.canEdit)
      this.controlValues
        .observe(parentPath(parentPath(this.rowModelPath())), parentView.canEdit)
        ?.subscribe((x) => this.gridEnabled.set(!!x));
    if (parentView.canEdit?.$type === 'constant' && !parentView.canEdit.value) return;
    if (parentView.canEditRow)
      this.controlValues
        .observe(this.rowModelPath(), parentView.canEditRow)
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
    if (this.controlType() !== ControlType.CheckBox) return;
    const current = this.control()?.value;
    this.control()?.setValue(!current);
    this.executeServiceMethod();
  }

  executeServiceMethod() {
    const method = this.metadataLookup.getPropertyMetadata(this.path(), 'formServiceMethod');
    if (method) this.serviceMethodService.runMethod(this.rowModelPath(), method);
  }
}
