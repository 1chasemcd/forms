import { BaseViewDefinition, FieldDefinition, MetadataDefinition } from '../api/api.g';
import { FormContext } from './form-context';
import { Observable } from 'rxjs';

export interface ViewProcessor {
  process(view: BaseViewDefinition, context: FormContext, parentEnabled: Observable<boolean>): void;
}

export interface FieldProcessor {
  process(field: FieldDefinition, context: FormContext, parentEnabled: Observable<boolean>): void;
}

export interface MetadataProcessor {
  process(
    metadata: MetadataDefinition,
    field: FieldDefinition,
    context: FormContext,
    parentEnabled: Observable<boolean>,
  ): void;
}
