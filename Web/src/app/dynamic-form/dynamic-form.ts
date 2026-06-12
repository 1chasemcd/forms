import { Component, effect, inject, input, output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { DynamicView } from '../dynamic-view/dynamic-view';
import { FormModelService } from '../form-services/form-model-service';
import { ControlValueService } from '../form-services/control-value-service';
import { ServiceMethodService } from '../service-method/service-method-service';
import { ControlEnablementService } from '../form-services/control-enablement-service';
import { FormResultModel } from './form-stack-model';
import { ControlPath } from '../utils/form-utils';
import { DefaultMetadataProcessorsService } from '../metadata/default-metadata-processors-service';

@Component({
  selector: 'app-dynamic-form',
  imports: [ReactiveFormsModule, DynamicView],
  templateUrl: './dynamic-form.html',
  providers: [
    FormModelService,
    ControlValueService,
    ServiceMethodService,
    ControlEnablementService,
    DefaultMetadataProcessorsService,
  ],
  host: {
    class: 'min-[72rem]:max-w-6xl 2xl:max-w-3/4 w-full',
  },
})
export class DynamicForm {
  readonly formModel = input.required<Record<string, unknown> | null>();
  readonly viewId = input.required<number>();
  readonly modelPath = input.required<ControlPath>();
  readonly formClosed = output<FormResultModel>();

  private readonly formModelService = inject(FormModelService);
  private readonly defaultMetadataProcessors = inject(DefaultMetadataProcessorsService);

  get model() {
    return this.formModelService.model;
  }

  constructor() {
    this.defaultMetadataProcessors.initialize();
    this.formModelService.initialize();
    effect(() => {
      const m = this.formModel();
      if (m) this.formModelService.patchValues([], m);
    });
  }

  onSubmit() {
    const model = this.formModelService.toRecord([]);
    this.formClosed.emit({
      model: model,
      commitChanges: true,
    });
  }
}
