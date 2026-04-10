import { Injectable } from '@angular/core';
import {
  BaseViewDefinition,
  FieldDefinition,
  MetadataDefinition,
  MetadataType,
} from '../api/api.g';
import { FieldProcessor, MetadataProcessor, ViewProcessor } from './form-processor-interfaces';

@Injectable({ providedIn: 'root' })
export class FormRegistryService {
  private viewProcessors = new Map<string, ViewProcessor>();
  private fieldProcessors = new Map<string, FieldProcessor>();
  private metadataProcessors = new Map<MetadataType, MetadataProcessor>();

  registerViewProcessor(type: string, processor: ViewProcessor) {
    this.viewProcessors.set(type.toLowerCase(), processor);
  }

  registerFieldProcessor(type: string, processor: FieldProcessor) {
    this.fieldProcessors.set(type.toLowerCase(), processor);
  }

  registerMetadataProcessor(type: MetadataType, processor: MetadataProcessor) {
    this.metadataProcessors.set(type, processor);
  }

  getViewProcessor(view: BaseViewDefinition): ViewProcessor | undefined {
    return this.viewProcessors.get(view.$type.toLowerCase());
  }

  getFieldProcessor(field: FieldDefinition): FieldProcessor | undefined {
    return this.fieldProcessors.get(field.type.toLowerCase());
  }

  getMetadataProcessor(metadata: MetadataDefinition): MetadataProcessor | undefined {
    return this.metadataProcessors.get(metadata.type);
  }
}
