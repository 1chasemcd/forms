import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { widthToCss } from '../../utils/width-utils';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ServiceMethodService } from '../../service-method/service-method-service';
import { Button } from '../button/button';
import { Checkbox } from '../checkbox/checkbox';
import { CustomLabelValue } from '../label-value/label-value';
import { DynamicInput } from '../dynamic-input/dynamic-input';
import { StandardInputWrapper } from '../standard-input/standard-input-wrapper';
import { ControlType, FormControlInfoContainer } from '../../api/api.g';
import { FormModelService } from '../../dynamic-form/form-model-service';
import { MetadataLookupService } from '../../metadata/metadata-lookup-service';
import { ControlPath } from '../../utils/form-utils';
import { PropertyOrConstantEvaluationService } from '../../dynamic-form/property-or-constant-evaluation-service';

@Component({
  selector: 'app-dynamic-control',
  host: {
    '[class]': 'width() + " content-center"',
  },
  templateUrl: './dynamic-control.html',
  imports: [
    Button,
    Checkbox,
    ReactiveFormsModule,
    CustomLabelValue,
    DynamicInput,
    StandardInputWrapper,
  ],
})
export class DynamicControl implements OnInit {
  private readonly formModelService = inject(FormModelService);
  private readonly metadataLookup = inject(MetadataLookupService);
  private readonly pocEvaluator = inject(PropertyOrConstantEvaluationService);
  private readonly serviceMethodService = inject(ServiceMethodService);

  readonly controlInfo = input.required<FormControlInfoContainer>();
  readonly parentPath = input.required<ControlPath>();

  private readonly path = computed(() => [...this.parentPath(), this.controlInfo().propertyName]);
  readonly width = computed(() => widthToCss(this.controlInfo().width));
  readonly control = computed(() => this.formModelService.get<FormControl>(this.path()));
  readonly controlType = computed(
    () => this.metadataLookup.getPropertyMetadata(this.path(), 'controlType') ?? ControlType.Text,
  );
  readonly visible = signal(true);
  readonly label = signal('');

  ngOnInit() {
    const visibleMetadata = this.metadataLookup.getPropertyMetadata(this.path(), 'visible');
    if (visibleMetadata)
      this.pocEvaluator
        .observe<boolean>(visibleMetadata, this.parentPath())
        .subscribe(this.visible.set);
    this.pocEvaluator
      .observe<string>(this.metadataLookup.getLabelMetadata(this.path()), this.parentPath())
      .subscribe(this.label.set);
  }

  executeServiceMethod() {
    const method = this.metadataLookup.getPropertyMetadata(this.path(), 'formServiceMethod');
    if (method) this.serviceMethodService.runMethod(this.parentPath(), method);
  }
}
