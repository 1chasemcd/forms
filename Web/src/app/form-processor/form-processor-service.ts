import { inject, Injectable } from '@angular/core';
import { BaseViewDefinition, FieldDefinition, FieldType } from '../api/api.g';
import { FormRegistryService } from './form-registry-service';
import { FormContext } from '../dynamic-form/form-context';
import { FormFieldEnablementService } from './form-field-enablement-service';
import { getPocObservable } from '../utils/api-utils';
import { AbstractControl } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class FormProcessorService {
  private readonly registry = inject(FormRegistryService);
  private readonly enablementService = inject(FormFieldEnablementService);

  processView(view: BaseViewDefinition, context: FormContext) {
    const processor = this.registry.getViewProcessor(view);

    if (!processor) {
      console.warn(`No view processor found for type: ${view.$type}`);
      return;
    }

    if (view.enabled)
      this.enablementService.enabledFor(view, getPocObservable(view.enabled, context));

    processor.process(view, context);
  }

  processField(field: FieldDefinition, context: FormContext): AbstractControl | null {
    let control: AbstractControl | null = null;
    if (field.type !== FieldType.Button) {
      control = context.getOrAddControl(field.property);
      this.enablementService.registerControl(control);
    }

    field.fieldMetadatas?.forEach((m) => {
      const metadataProcessor = this.registry.getMetadataProcessor(m);
      if (metadataProcessor) {
        metadataProcessor.process(m, field, context);
      }
    });

    return control;
  }
}
