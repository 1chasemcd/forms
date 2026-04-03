import { inject, Injectable, Type } from '@angular/core';
import { FieldType } from '../api/api.g';
import { CUSTOM_FIELDS } from './custom-field-provider';
import { CustomButtonComponent, CustomInputComponent } from './custom-field-registration';

@Injectable({ providedIn: 'root' })
export class CustomFieldService {
  private registry = inject(CUSTOM_FIELDS);

  getField<T = CustomInputComponent | CustomButtonComponent>(type: FieldType): Type<T> {
    const found = this.registry.find((r) => r.type === type);
    if (!found) throw new Error(`Could not find custom registration for type ${type}`);
    return found.component as Type<T>;
  }
}
