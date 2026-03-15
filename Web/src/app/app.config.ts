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
import { CustomText } from './custom-field/custom-text/custom-text';
import { CustomButton } from './custom-field/custom-button/custom-button';
import { CustomCheckbox } from './custom-field/custom-checkbox/custom-checkbox';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    provideCustomStaticText(CustomText),
    provideCustomButton(CustomButton),
    provideCustomInput('checkboxinput', CustomCheckbox),
  ],
};
