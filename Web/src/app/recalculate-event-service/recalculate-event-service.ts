import { inject, Injectable } from '@angular/core';
import { FormDefinition, RecalculateEvent, RecalculateEventClient } from '../api/api.g';
import { catchError, throwError } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { FormValueService } from '../dynamic-form/form-value-service';

@Injectable({ providedIn: 'root' })
export class RecalculateEventService {
  private recalculateEventClient = inject(RecalculateEventClient);
  private formValueService = inject(FormValueService);

  runRecalculate(
    model: FormGroup,
    recalculateEvent: RecalculateEvent,
    formDefinition?: FormDefinition,
  ) {
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
        if (formDefinition) {
          this.formValueService.patchValues(model, result.model, formDefinition);
        } else {
          // Fallback if formDefinition is not provided - might not work for grids
          for (const [key, value] of Object.entries(result.model as Record<string, unknown>)) {
            model.get(key)?.setValue(value);
          }
        }
      });
  }
}
