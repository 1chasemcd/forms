import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FileResponse, FormDefinitionClient, FormDefinition, RepositoryClient } from '../api/api.g';
import { ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { catchError, of, throwError } from 'rxjs';
import { DynamicView } from '../dynamic-view/dynamic-view';
import { FormValueService } from './form-value-service';
import { FormFactory } from './form-factory';
import { StandardProcessorsService } from '../form-processor/standard-processors.service';
import { FormContext } from './form-context';
import { GridRowFactory } from './grid-row-factory';

@Component({
  selector: 'app-dynamic-form',
  imports: [ReactiveFormsModule, DynamicView],
  templateUrl: './dynamic-form.html',
  providers: [FormDefinitionClient, RepositoryClient, FormFactory],
})
export class DynamicForm implements OnInit, OnDestroy {
  private readonly formDefinitionClient = inject(FormDefinitionClient);
  private readonly repositoryClient = inject(RepositoryClient);
  private readonly formFactory = inject(FormFactory);
  private readonly formValueService = inject(FormValueService);
  private readonly standardProcessors = inject(StandardProcessorsService);
  private readonly route = inject(ActivatedRoute);
  private readonly gridRowFactory = inject(GridRowFactory);

  formDefinition?: FormDefinition;
  formContext?: FormContext;

  ngOnInit() {
    this.standardProcessors.register();
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

  ngOnDestroy() {
    this.gridRowFactory.clear();
  }

  private handleFormPathNotFound() {
    console.log('Form not found');
  }

  private handleFormResponse(form: FormDefinition | null) {
    if (form == null) return;
    this.formDefinition = form;
    this.formContext = this.formFactory.createFormContext(form.view);
    if (form.modelType)
      this.repositoryClient
        .create(form.modelType)
        .subscribe((r) => this.handleRepositoryResponse(r));
  }

  private handleRepositoryResponse(resp: FileResponse) {
    resp.data.text().then((text) => {
      if (this.formContext && this.formDefinition)
        this.formValueService.patchValues(this.formContext, JSON.parse(text));
    });
  }

  onSubmit() {
    // Do stuff
  }
}
