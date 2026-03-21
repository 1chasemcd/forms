import { inject, Injectable } from '@angular/core';
import { RecalculateEvent, RecalculateEventClient } from '../api/api.g';
import { FormModel } from '../dynamic-form/form-model';
import { catchError, throwError } from 'rxjs';

@Injectable()
export class RecalculateEventService {
  private recalculateEventClient = inject(RecalculateEventClient);

  runRecalculate(model: FormModel, recalculateEvent: RecalculateEvent) {
    let propertiesToSend = {};
    if (!recalculateEvent.DontSendModel) propertiesToSend = model.asRecord();

    this.recalculateEventClient
      .performAction(recalculateEvent.Service, recalculateEvent.Method, propertiesToSend)
      .pipe(
        catchError((error) => {
          console.log(error);
          return throwError(() => error);
        }),
      )
      .subscribe((result) => model.patch(result.Model));
  }
}
