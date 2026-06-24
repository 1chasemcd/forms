import { inject, Injectable } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ServiceMethod, ServiceMethodClient } from '../api/api.g';
import { ControlPath } from '../utils/form-utils';
import { FormStackService } from '../form/form-services/form-stack-service';

@Injectable()
export class ServiceMethodService {
  private serviceMethodClient = inject(ServiceMethodClient);
  private formStack = inject(FormStackService);

  runMethod(modelPath: ControlPath, serviceMethod: ServiceMethod) {
    const propertiesToSend = this.formStack.activeModel.toRecord(modelPath);

    this.serviceMethodClient
      .runMethod(serviceMethod.service, serviceMethod.method, propertiesToSend)
      .pipe(
        catchError((error) => {
          console.log(error);
          return throwError(() => error);
        }),
      )
      .subscribe((result) => {
        this.formStack.activeModel.patchValues(result.model, modelPath);
      });
  }
}
