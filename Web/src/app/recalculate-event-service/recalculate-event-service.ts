import { inject, Injectable } from '@angular/core';
import { RecalculateEvent, RecalculateEventClient } from '../api/api.g';
import { catchError, throwError } from 'rxjs';
import { FormValueService } from '../dynamic-form/form-value-service';
import { FormContext } from '../dynamic-form/form-context';

@Injectable({ providedIn: 'root' })
export class RecalculateEventService {
  private recalculateEventClient = inject(RecalculateEventClient);
  private formValueService = inject(FormValueService);

  runRecalculate(context: FormContext, recalculateEvent: RecalculateEvent) {
    const propertiesToSend = this.formValueService.toRecord(context.formGroup);

    this.recalculateEventClient
      .performAction(recalculateEvent.service, recalculateEvent.method, propertiesToSend)
      .pipe(
        catchError((error) => {
          console.log(error);
          return throwError(() => error);
        }),
      )
      .subscribe((result) => {
        this.formValueService.patchValues(context, result.model);
      });
  }
}
