import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withXhr } from '@angular/common/http';
import { FormClient, RepositoryClient, ServiceMethodClient } from './api/api.g';
export const appConfig: ApplicationConfig = {
  providers: [
    ServiceMethodClient,
    RepositoryClient,
    FormClient,
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withXhr()),
  ],
};
