import { inject, Injectable } from '@angular/core';
import { RecalculateEvent, RecalculateEventClient } from '../api/api.g';
import { FormModel } from '../dynamic-form/form-model';
import { catchError, throwError } from 'rxjs';

@Injectable()
export class RecalculateEventService {
  private recalculateEventClient = inject(RecalculateEventClient);

  runRecalculate(model: FormModel, recalculateEvent: RecalculateEvent) {
    let propertiesToSend = {};
    if (recalculateEvent.PropertiesToSend.$type == 'sendsome')
      propertiesToSend = this.pickKeys(model.asRecord(), recalculateEvent.PropertiesToSend.Names);
    else if (recalculateEvent.PropertiesToSend.$type == 'sendall')
      propertiesToSend = model.asRecord();

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

  private pickKeys(rec: Record<string, unknown>, keysToTake: string[]): Record<string, unknown> {
    return Object.fromEntries(Object.entries(rec).filter(([key]) => keysToTake.includes(key)));
  }
}
