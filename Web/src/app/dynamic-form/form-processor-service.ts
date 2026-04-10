import { inject, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BaseViewDefinition, FieldDefinition, FormDefinition } from '../api/api.g';
import { FormRegistryService } from './form-registry-service';
import { FormContext } from './form-context';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FormProcessorService {
  private registry = inject(FormRegistryService);

  processForm(formDefinition: FormDefinition, formGroup: FormGroup, context: FormContext) {
    if (formDefinition.view) {
      this.processView(formDefinition.view, formGroup, context, of(true));
    }
  }

  processView(
    view: BaseViewDefinition,
    formGroup: FormGroup,
    context: FormContext,
    parentEnabled: Observable<boolean>,
  ) {
    const processor = this.registry.getViewProcessor(view);
    if (processor) {
      processor.process(view, formGroup, context, parentEnabled);
    } else {
      console.warn(`No view processor found for type: ${view.$type}`);
    }
  }

  processField(
    field: FieldDefinition,
    formGroup: FormGroup,
    context: FormContext,
    parentEnabled: Observable<boolean>,
  ) {
    const processor = this.registry.getFieldProcessor(field);
    if (processor) {
      processor.process(field, formGroup, context, parentEnabled);
    } else {
      console.warn(`No field processor found for type: ${field.type}`);
    }

    field.fieldMetadatas?.forEach((m) => {
      const metadataProcessor = this.registry.getMetadataProcessor(m);
      if (metadataProcessor) {
        metadataProcessor.process(m, field, formGroup, context, parentEnabled);
      }
    });
  }
}
