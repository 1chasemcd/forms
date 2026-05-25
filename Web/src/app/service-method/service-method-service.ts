import { inject, Injectable } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ServiceMethod, ServiceMethodClient } from '../api/api.g';
import { FormModelService } from '../dynamic-form/form-model-service';
import { ControlPath } from '../utils/form-utils';

@Injectable({ providedIn: 'root' })
export class ServiceMethodService {
  private serviceMethodClient = inject(ServiceMethodClient);
  private formModelService = inject(FormModelService);

  runMethod(modelPath: ControlPath, serviceMethod: ServiceMethod) {
    const propertiesToSend = this.formModelService.toRecord(modelPath);

    this.serviceMethodClient
      .runMethod(serviceMethod.service, serviceMethod.method, propertiesToSend)
      .pipe(
        catchError((error) => {
          console.log(error);
          return throwError(() => error);
        }),
      )
      .subscribe((result) => {
        this.formModelService.patchValues(modelPath, result.model);
      });
  }
}
