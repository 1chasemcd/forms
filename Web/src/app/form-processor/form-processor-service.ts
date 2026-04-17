import { inject, Injectable } from '@angular/core';
import { BaseViewDefinition, FieldDefinition } from '../api/api.g';
import { FormRegistryService } from './form-registry-service';
import { FormContext } from '../dynamic-form/form-context';

@Injectable({ providedIn: 'root' })
export class FormProcessorService {
  private registry = inject(FormRegistryService);

  processView(view: BaseViewDefinition, context: FormContext) {
    const processor = this.registry.getViewProcessor(view);
    if (processor) {
      processor.process(view, context);
    } else {
      console.warn(`No view processor found for type: ${view.$type}`);
    }
  }

  processField(field: FieldDefinition, context: FormContext) {
    const processor = this.registry.getFieldProcessor(field);
    if (processor) {
      processor.process(field, context);
    } else {
      console.warn(`No field processor found for type: ${field.type}`);
    }

    field.fieldMetadatas?.forEach((m) => {
      const metadataProcessor = this.registry.getMetadataProcessor(m);
      if (metadataProcessor) {
        metadataProcessor.process(m, field, context);
      }
    });
  }
}
