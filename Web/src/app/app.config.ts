import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import {
  provideCustomButton,
  provideCustomInput,
  provideCustomStaticText,
} from './field-resolution/custom-field-provider';
import { CustomStaticText } from './custom-field/custom-static-text/custom-static-text';
import { CustomButton } from './custom-field/custom-button/custom-button';
import { CustomCheckbox } from './custom-field/custom-checkbox/custom-checkbox';
import { CustomTextInput } from './custom-field/custom-text-input/custom-text-input';
import { CustomNumberInput } from './custom-field/custom-number-input/custom-number-input';
import { CustomDateInput } from './custom-field/custom-date-input/custom-date-input';
import { CustomTextAreaInput } from './custom-field/custom-text-area-input/custom-text-area-input';
import { CustomTimeInput } from './custom-field/custom-time-input/custom-time-input';
import { FieldType, RecalculateEventClient } from './api/api.g';

export const appConfig: ApplicationConfig = {
  providers: [
    RecalculateEventClient,
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    provideCustomStaticText(CustomStaticText),
    provideCustomButton(CustomButton),
    provideCustomInput(FieldType.CheckBox, CustomCheckbox),
    provideCustomInput(FieldType.Text, CustomTextInput),
    provideCustomInput(FieldType.Numeric, CustomNumberInput),
    provideCustomInput(FieldType.Currency, CustomNumberInput),
    provideCustomInput(FieldType.Date, CustomDateInput),
    provideCustomInput(FieldType.TextArea, CustomTextAreaInput),
    provideCustomInput(FieldType.Time, CustomTimeInput),
  ],
};
