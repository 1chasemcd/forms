import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideCustomButton, provideCustomInput } from './field-resolution/custom-field-provider';
import { CustomLabelValue } from './custom-field/custom-label-value/custom-label-value';
import { CustomButton } from './custom-field/custom-button/custom-button';
import { CustomCheckbox } from './custom-field/custom-checkbox/custom-checkbox';
import { CustomNumberInput } from './custom-field/custom-number-input/custom-number-input';
import { CustomTextArea } from './custom-field/custom-text-area/custom-text-area';
import { FieldType, RecalculateEventClient } from './api/api.g';
import { createStandardCustomInput } from './custom-field/standard-custom-input';

export const appConfig: ApplicationConfig = {
  providers: [
    RecalculateEventClient,
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    provideCustomButton(CustomButton),
    provideCustomInput(FieldType.LabelValue, CustomLabelValue),
    provideCustomInput(FieldType.CheckBox, CustomCheckbox),
    provideCustomInput(FieldType.Text, createStandardCustomInput('text')),
    provideCustomInput(FieldType.Numeric, CustomNumberInput),
    provideCustomInput(FieldType.Currency, CustomNumberInput),
    provideCustomInput(FieldType.Date, createStandardCustomInput('date')),
    provideCustomInput(FieldType.TextArea, CustomTextArea),
    provideCustomInput(FieldType.Time, createStandardCustomInput('time')),
  ],
};
