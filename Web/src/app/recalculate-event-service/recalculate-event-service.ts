import { inject, Injectable } from '@angular/core';
import { RecalculateEvent, RecalculateEventClient } from '../api/api.g';
import { catchError, throwError } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { FormModelService } from '../dynamic-form/form-model-service';

@Injectable()
export class RecalculateEventService {
  private recalculateEventClient = inject(RecalculateEventClient);
  private formModelService = inject(FormModelService);

  runRecalculate(model: FormGroup, recalculateEvent: RecalculateEvent) {
    const propertiesToSend = this.formModelService.toRecord(model);

    this.recalculateEventClient
      .performAction(recalculateEvent.service, recalculateEvent.method, propertiesToSend)
      .pipe(
        catchError((error) => {
          console.log(error);
          return throwError(() => error);
        }),
      )
      .subscribe((result) => this.formModelService.patchValues(model, result.model));
  }
}
