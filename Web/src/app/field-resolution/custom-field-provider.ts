import { InjectionToken, Provider, Type } from '@angular/core';
import {
  CustomButtonComponent,
  CustomFieldRegistration,
  CustomInputComponent,
} from './custom-field-registration';
import { FieldType } from '../api/api.g';

export const CUSTOM_FIELDS = new InjectionToken<CustomFieldRegistration[]>('CUSTOM_FIELDS');

export function provideCustomInput(
  type: FieldType,
  component: Type<CustomInputComponent>,
): Provider {
  return {
    provide: CUSTOM_FIELDS,
    useValue: { type, component },
    multi: true,
  };
}

export function provideCustomButton(component: Type<CustomButtonComponent>): Provider {
  const type = FieldType.Button;
  return {
    provide: CUSTOM_FIELDS,
    useValue: { type, component },
    multi: true,
  };
}
