import { inject, Injectable } from '@angular/core';
import { RecalculateEvent, RecalculateEventClient } from '../api/api.g';
import { catchError, throwError } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { FormValueService } from '../dynamic-form/form-value-service';

@Injectable({ providedIn: 'root' })
export class RecalculateEventService {
  private recalculateEventClient = inject(RecalculateEventClient);
  private formValueService = inject(FormValueService);

  runRecalculate(model: FormGroup, recalculateEvent: RecalculateEvent) {
    const propertiesToSend = this.formValueService.toRecord(model);

    this.recalculateEventClient
      .performAction(recalculateEvent.service, recalculateEvent.method, propertiesToSend)
      .pipe(
        catchError((error) => {
          console.log(error);
          return throwError(() => error);
        }),
      )
      .subscribe((result) => {
        this.formValueService.patchValues(model, result.model);
      });
  }
}
