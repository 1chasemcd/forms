import { Component, computed, inject, input, linkedSignal, OnInit, signal } from '@angular/core';
import { widthToCss } from '../../utils/width-utils';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ServiceMethodService } from '../../service-method/service-method-service';
import { Button } from '../button/button';
import { CustomLabelValue } from '../label-value/label-value';
import { DynamicInput } from '../dynamic-input/dynamic-input';
import { StandardInputWrapper } from '../standard-input/standard-input-wrapper';
import { ControlType, FormControlInfoContainer } from '../../api/api.g';
import { FormModelService } from '../../form/form-services/form-model-service';
import { MetadataLookupService } from '../../metadata/metadata-lookup-service';
import { ControlPath, joinPath } from '../../utils/form-utils';
import { ControlValueService } from '../../form/form-services/control-value-service';
import { pascalCaseToWords } from '../../utils/string-utils';
import { Checkbox } from '../checkbox/checkbox';

@Component({
  selector: 'app-dynamic-control',
  host: {
    class: 'content-center',
    '[class]': 'width()',
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
  private readonly controlValues = inject(ControlValueService);
  private readonly serviceMethodService = inject(ServiceMethodService);

  readonly controlInfo = input.required<FormControlInfoContainer>();
  readonly parentPath = input.required<ControlPath>();

  private readonly path = computed(() =>
    joinPath(this.parentPath(), this.controlInfo().propertyName),
  );
  readonly width = computed(() => widthToCss(this.controlInfo().width));
  readonly control = computed(() => this.formModelService.get<FormControl>(this.path()));
  readonly controlType = computed(
    () => this.metadataLookup.getPropertyMetadata(this.path(), 'controlType') ?? ControlType.Text,
  );
  readonly visible = signal(true);
  readonly label = linkedSignal(() => pascalCaseToWords(this.controlInfo().propertyName));

  ngOnInit() {
    this.controlValues
      .observe<boolean>(this.path(), 'visible')
      ?.subscribe((v) => this.visible.set(v));
    this.controlValues
      .observeMetadata<string>(this.path(), 'label')
      ?.subscribe((l) => this.label.set(l));
  }

  executeServiceMethod() {
    const method = this.metadataLookup.getPropertyMetadata(this.path(), 'formServiceMethod');
    if (method) this.serviceMethodService.runMethod(this.parentPath(), method);
  }
}
