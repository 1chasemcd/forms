import { InjectionToken, Provider, Type } from '@angular/core';
import {
  CustomButtonComponent,
  CustomFieldRegistration,
  CustomInputComponent,
  CustomStaticTextComponent,
} from './custom-field-registration';
import { BaseInputType } from '../utils/api-utils';

export const CUSTOM_FIELDS = new InjectionToken<CustomFieldRegistration[]>('CUSTOM_FIELDS');

export function provideCustomInput(
  type: BaseInputType,
  component: Type<CustomInputComponent>,
): Provider {
  return {
    provide: CUSTOM_FIELDS,
    useValue: { type, component },
    multi: true,
  };
}

export function provideCustomButton(component: Type<CustomButtonComponent>): Provider {
  const type = 'buttonfield';
  return {
    provide: CUSTOM_FIELDS,
    useValue: { type, component },
    multi: true,
  };
}

export function provideCustomStaticText(component: Type<CustomStaticTextComponent>): Provider {
  const type = 'statictextfield';
  return {
    provide: CUSTOM_FIELDS,
    useValue: { type, component },
    multi: true,
  };
}
