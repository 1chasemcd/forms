import { Injectable } from '@angular/core';
import { BaseViewDefinition, MetadataDefinition, MetadataType } from '../api/api.g';
import { MetadataProcessor, ViewProcessor } from './form-processor-interfaces';

@Injectable({ providedIn: 'root' })
export class FormRegistryService {
  private viewProcessors = new Map<string, ViewProcessor>();
  private metadataProcessors = new Map<MetadataType, MetadataProcessor>();

  registerViewProcessor(type: string, processor: ViewProcessor) {
    this.viewProcessors.set(type.toLowerCase(), processor);
  }

  registerMetadataProcessor(type: MetadataType, processor: MetadataProcessor) {
    this.metadataProcessors.set(type, processor);
  }

  getViewProcessor(view: BaseViewDefinition): ViewProcessor | undefined {
    return this.viewProcessors.get(view.$type);
  }

  getMetadataProcessor(metadata: MetadataDefinition): MetadataProcessor | undefined {
    return this.metadataProcessors.get(metadata.type);
  }
}
