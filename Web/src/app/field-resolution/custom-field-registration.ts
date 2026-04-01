import { InputSignal, OutputEmitterRef, Type } from '@angular/core';
import { FormControl } from '@angular/forms';
import { FieldType } from '../api/api.g';

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
  type: FieldType;
  component: Type<CustomInputComponent>;
}

export interface CustomButtonRegistration {
  type: FieldType.Button;
  component: Type<CustomButtonComponent>;
}

export interface CustomStaticTextRegistration {
  type: FieldType.LabelValue;
  component: Type<CustomStaticTextComponent>;
}

export type CustomFieldRegistration =
  | CustomInputRegistration
  | CustomButtonRegistration
  | CustomStaticTextRegistration;
