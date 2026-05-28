import { Injectable } from '@angular/core';
import { PropertyMetadata } from '../api/api.g';
import { AbstractControl } from '@angular/forms';
import { MetadataByType, MetadataType } from '../utils/api-utils';

export interface MetadataProcessor<T extends PropertyMetadata> {
  process(control: AbstractControl, metadata: T): void;
}

@Injectable()
export class MetadataProcessorRegistryService {
  private metadataProcessors = new Map<string, MetadataProcessor<PropertyMetadata>>();

  registerMetadataProcessor<T extends MetadataType>(
    type: T,
    processor: MetadataProcessor<MetadataByType<T>>,
  ) {
    this.metadataProcessors.set(type, processor);
  }

  getMetadataProcessor(
    metadata: PropertyMetadata,
  ): MetadataProcessor<PropertyMetadata> | undefined {
    return this.metadataProcessors.get(metadata.$type);
  }
}
