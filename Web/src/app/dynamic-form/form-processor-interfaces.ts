import { FormGroup } from '@angular/forms';
import { BaseViewDefinition, FieldDefinition, MetadataDefinition } from '../api/api.g';
import { FormContext } from './form-context';
import { Observable } from 'rxjs';

export interface ViewProcessor {
  process(
    view: BaseViewDefinition,
    formGroup: FormGroup,
    context: FormContext,
    parentEnabled: Observable<boolean>,
  ): void;
}

export interface FieldProcessor {
  process(
    field: FieldDefinition,
    formGroup: FormGroup,
    context: FormContext,
    parentEnabled: Observable<boolean>,
  ): void;
}

export interface MetadataProcessor {
  process(
    metadata: MetadataDefinition,
    field: FieldDefinition,
    formGroup: FormGroup,
    context: FormContext,
    parentEnabled: Observable<boolean>,
  ): void;
}
