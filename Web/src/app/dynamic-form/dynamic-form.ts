import { Component, inject, OnInit } from '@angular/core';
import { FileResponse, FormDefinitionClient, FormDefinition, RepositoryClient } from '../api/api.g';
import { FormControlService } from './form-control-service';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { catchError, of, throwError } from 'rxjs';
import { DynamicView } from '../dynamic-view/dynamic-view';
import { FormModelService } from './form-model-service';
import { GridDefinitionService } from './grid-definition-service';
import { RecalculateEventService } from '../recalculate-event-service/recalculate-event-service';

@Component({
  selector: 'app-dynamic-form',
  imports: [ReactiveFormsModule, DynamicView],
  templateUrl: './dynamic-form.html',
  providers: [
    FormDefinitionClient,
    RepositoryClient,
    FormControlService,
    FormModelService,
    GridDefinitionService,
    RecalculateEventService,
  ],
})
export class DynamicForm implements OnInit {
  private readonly formDefinitionClient = inject(FormDefinitionClient);
  private readonly repositoryClient = inject(RepositoryClient);
  private readonly formControlService = inject(FormControlService);
  private readonly formModelService = inject(FormModelService);
  private readonly route = inject(ActivatedRoute);
  formDefinition?: FormDefinition;
  formGroup?: FormGroup;

  ngOnInit() {
    const path = this.route.snapshot.paramMap.get('path');
    if (path == null) return;

    this.formDefinitionClient
      .getForm(path)
      .pipe(
        catchError((error) => {
          if (error.status == 404) {
            this.handleFormPathNotFound();
            return of(null);
          }
          return throwError(() => error);
        }),
      )
      .subscribe((f) => this.handleFormResponse(f));
  }

  private handleFormPathNotFound() {
    console.log('Form not found');
  }

  private handleFormResponse(form: FormDefinition | null) {
    if (form == null) return;
    this.formDefinition = form;
    this.formGroup = this.formControlService.createFromDefinition(form);
    if (form.modelType)
      this.repositoryClient
        .create(form.modelType)
        .subscribe((r) => this.handleRepositoryResponse(r));
  }

  private handleRepositoryResponse(resp: FileResponse) {
    resp.data.text().then((text) => {
      if (this.formGroup) this.formModelService.patchValues(this.formGroup, JSON.parse(text));
    });
  }

  onSubmit() {
    // Do stuff
  }
}
