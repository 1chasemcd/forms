import { DestroyRef, inject, Injectable } from '@angular/core';
import { BaseViewDefinition } from '../api/api.g';
import { FormProcessorService } from '../form-processor/form-processor-service';
import { FormContext } from './form-context';

@Injectable({ providedIn: 'root' })
export class FormFactory {
  private processorService = inject(FormProcessorService);
  private destroyRef = inject(DestroyRef);

  createFormContext(view: BaseViewDefinition): FormContext {
    const context = new FormContext(this.destroyRef);
    this.processorService.processView(view, context);
    return context;
  }
}
