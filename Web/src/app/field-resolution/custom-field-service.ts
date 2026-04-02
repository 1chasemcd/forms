import { inject, Injectable } from '@angular/core';
import { FieldType } from '../api/api.g';
import { CUSTOM_FIELDS } from './custom-field-provider';

@Injectable({ providedIn: 'root' })
export class CustomFieldService {
  private registry = inject(CUSTOM_FIELDS);

  getField(type: FieldType) {
    return this.registry.find((r) => r.type === type)?.component;
  }
}
