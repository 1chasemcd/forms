import { Component, computed, inject, input, linkedSignal, OnInit, signal } from '@angular/core';
import { widthToCss } from '../../utils/width-utils';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ServiceMethodService } from '../../service-method/service-method-service';
import { LabelValue } from '../label-value/label-value';
import { FieldType, FormFieldInfoContainer } from '../../api/api.g';
import { MetadataLookupService } from '../../metadata/metadata-lookup';
import { ControlPath, joinPath } from '../../utils/form-utils';
import { pascalCaseToWords } from '../../utils/string-utils';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateInput } from '../date-input/date-input';
import { FormStackService } from '../../form/form-services/form-stack-service';

@Component({
  selector: 'app-dynamic-field',
  host: {
    class: 'content-center',
    '[class]': 'width()',
  },
  templateUrl: './dynamic-field.html',
  imports: [
    ReactiveFormsModule,
    LabelValue,
    MatCheckboxModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    DateInput,
  ],
})
export class DynamicField implements OnInit {
  private readonly formStack = inject(FormStackService);
  private readonly metadataLookup = inject(MetadataLookupService);
  private readonly serviceMethodService = inject(ServiceMethodService);

  readonly fieldInfo = input.required<FormFieldInfoContainer>();
  readonly parentPath = input.required<ControlPath>();

  readonly path = computed(() => joinPath(this.parentPath(), this.fieldInfo().identifier));
  readonly width = computed(() => widthToCss(this.fieldInfo().width));
  readonly control = computed(() => this.formStack.activeModel.get<FormControl>(this.path()));
  readonly fieldType = computed(
    () =>
      this.formStack.activeModel.valueRefAugmentor.getMetadataValue(this.path(), 'fieldType') ??
      FieldType.Text,
  );
  readonly visible = signal(true);
  readonly label = linkedSignal(() => pascalCaseToWords(this.fieldInfo().identifier));

  ngOnInit() {
    this.formStack.activeModel.valueRefAugmentor
      .getMetadataValue<boolean>(this.path(), 'visible')
      ?.subscribe((v) => this.visible.set(v));
    this.formStack.activeModel.valueRefAugmentor
      .getMetadataValue<string>(this.path(), 'label')
      ?.subscribe((l) => this.label.set(l));
  }

  executeServiceMethod() {
    const method = this.formStack.activeModel.valueRefAugmentor.getMetadataValue(
      this.path(),
      'formServiceMethod',
    );
    if (method) this.serviceMethodService.runMethod(this.parentPath(), method);
  }
}
