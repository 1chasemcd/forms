import { BaseViewDefinition, FieldDefinition, MetadataDefinition } from '../api/api.g';
import { FormContext } from './form-context';

export interface ViewProcessor {
  process(view: BaseViewDefinition, context: FormContext): void;
}

export interface FieldProcessor {
  process(field: FieldDefinition, context: FormContext): void;
}

export interface MetadataProcessor {
  process(metadata: MetadataDefinition, field: FieldDefinition, context: FormContext): void;
}
