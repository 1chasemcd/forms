import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FileResponse, FormDefinitionClient, FormDefinition, RepositoryClient } from '../api/api.g';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { catchError, of, throwError } from 'rxjs';
import { DynamicView } from '../dynamic-view/dynamic-view';
import { FormValueService } from './form-value-service';
import { FormFactory, GridRegistry } from './form-factory';
import { StandardProcessorsService } from './standard-processors.service';

@Component({
  selector: 'app-dynamic-form',
  imports: [ReactiveFormsModule, DynamicView],
  templateUrl: './dynamic-form.html',
  providers: [FormDefinitionClient, RepositoryClient, FormValueService, FormFactory, GridRegistry],
})
export class DynamicForm implements OnInit, OnDestroy {
  private readonly formDefinitionClient = inject(FormDefinitionClient);
  private readonly repositoryClient = inject(RepositoryClient);
  private readonly formFactory = inject(FormFactory);
  private readonly formValueService = inject(FormValueService);
  private readonly standardProcessors = inject(StandardProcessorsService);
  private readonly gridRegistry = inject(GridRegistry);
  private readonly route = inject(ActivatedRoute);

  formDefinition?: FormDefinition;
  formGroup?: FormGroup;

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
    this.gridRegistry.clear();
  }

  private handleFormPathNotFound() {
    console.log('Form not found');
  }

  private handleFormResponse(form: FormDefinition | null) {
    if (form == null) return;
    this.formDefinition = form;
    this.formGroup = this.formFactory.createFormGroup(form.view, form);
    if (form.modelType)
      this.repositoryClient
        .create(form.modelType)
        .subscribe((r) => this.handleRepositoryResponse(r));
  }

  private handleRepositoryResponse(resp: FileResponse) {
    resp.data.text().then((text) => {
      if (this.formGroup && this.formDefinition)
        this.formValueService.patchValues(this.formGroup, JSON.parse(text), this.formDefinition);
    });
  }

  onSubmit() {
    // Do stuff
  }
}
