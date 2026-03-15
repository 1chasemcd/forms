import { InputSignal, OutputEmitterRef, Type } from '@angular/core';
import { BaseInputType } from '../utils/api-utils';
import { ButtonField, StaticTextField } from '../api/api.g';
import { FormControl } from '@angular/forms';

export interface CustomInputComponent {
  label: InputSignal<string>;
  formControl: InputSignal<FormControl>;
}

export interface CustomButtonComponent {
  label: InputSignal<string>;
  disabled: InputSignal<boolean | undefined>;
  clicked: OutputEmitterRef<void>;
}

export interface CustomStaticTextComponent {
  label: InputSignal<string>;
}

export interface CustomInputRegistration {
  type: BaseInputType;
  component: Type<CustomInputComponent>;
}

export interface CustomButtonRegistration {
  type: ButtonField['$type'];
  component: Type<CustomButtonComponent>;
}

export interface CustomStaticTextRegistration {
  type: StaticTextField['$type'];
  component: Type<CustomStaticTextComponent>;
}

export type CustomFieldRegistration =
  | CustomInputRegistration
  | CustomButtonRegistration
  | CustomStaticTextRegistration;
