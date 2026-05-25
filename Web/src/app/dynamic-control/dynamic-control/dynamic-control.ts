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
import { FormModelService } from '../../form-services/form-model-service';
import { MetadataLookupService } from '../../metadata/metadata-lookup-service';
import { ControlPath } from '../../utils/form-utils';
import { PropertyOrConstantEvaluationService } from '../../form-services/property-or-constant-evaluation-service';
import { pascalCaseToWords } from '../../utils/string-utils';

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
    this.pocEvaluator
      .propertyMetadataValueChanges<boolean>(this.path(), 'visible')
      .subscribe((v) => this.visible.set(v ?? true));
    this.pocEvaluator
      .propertyMetadataValueChanges<string>(this.path(), 'label')
      .subscribe((l) => this.label.set(l ?? pascalCaseToWords(this.controlInfo().propertyName)));
  }

  executeServiceMethod() {
    const method = this.metadataLookup.getPropertyMetadata(this.path(), 'formServiceMethod');
    if (method) this.serviceMethodService.runMethod(this.parentPath(), method);
  }
}
