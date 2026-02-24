import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormClient, FormModel } from '../api/api.g';
import { FormControlService } from './form-control-service';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { catchError, of, throwError } from 'rxjs';
import { DynamicView } from '../view/dynamic-view/dynamic-view';
import { FormView } from '../api/types';

@Component({
  selector: 'app-dynamic-form',
  imports: [ReactiveFormsModule, DynamicView],
  templateUrl: './dynamic-form.html',
  providers: [FormClient, FormControlService],
})
export class DynamicForm implements OnInit {
  private readonly formClient = inject(FormClient);
  private readonly formControlService = inject(FormControlService);
  private readonly route = inject(ActivatedRoute);
  formGroup = signal<FormGroup>(new FormGroup({}));
  formModel = signal<FormModel>({});
  formView = computed(() => this.formModel()?.view as FormView);

  ngOnInit() {
    const path = this.route.snapshot.paramMap.get('path');
    if (path == null) return;

    this.formClient
      .getForm(path)
      .pipe(
        catchError((error) => {
          if (error.status == 404) {
            console.log('Form not found');
            return of(null);
          }
          return throwError(() => error);
        }),
      )
      .subscribe((f) => {
        if (f == null) return;
        this.formModel.set(f);
        this.formGroup.set(this.formControlService.getFormGroup(f));
      });
  }

  onSubmit() {
    // Do stuff
  }
}
