import { Component, inject, OnInit } from '@angular/core';
import { FileResponse, FormClient, FormResponse, RepositoryClient } from '../api/api.g';
import { ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { catchError, of, throwError } from 'rxjs';
import { DynamicView } from '../dynamic-view/dynamic-view';
import { MetadataLookupService } from '../metadata/metadata-lookup-service';
import { MetadataProcessorRegistryService } from '../metadata/metadata-processor-registry-service';
import { ViewLookupService } from '../form-services/view-lookup-service';
import { FormModelService } from '../form-services/form-model-service';
import { PropertyOrConstantEvaluationService } from '../form-services/property-or-constant-evaluation-service';
import { ServiceMethodService } from '../service-method/service-method-service';
import { ControlEnablementService } from '../form-services/control-enablement-service';

@Component({
  selector: 'app-dynamic-form',
  imports: [ReactiveFormsModule, DynamicView],
  templateUrl: './dynamic-form.html',
  providers: [
    MetadataLookupService,
    MetadataProcessorRegistryService,
    ViewLookupService,
    FormModelService,
    PropertyOrConstantEvaluationService,
    ServiceMethodService,
    ControlEnablementService,
  ],
})
export class DynamicForm implements OnInit {
  private readonly formClient = inject(FormClient);
  private readonly repositoryClient = inject(RepositoryClient);
  private readonly formModelService = inject(FormModelService);
  private readonly metadataProcessorRegistry = inject(MetadataProcessorRegistryService);
  private readonly route = inject(ActivatedRoute);
  private readonly metadataLookup = inject(MetadataLookupService);
  private readonly viewLookup = inject(ViewLookupService);

  private _initialized = false;
  get initialized() {
    return this._initialized;
  }
  get model() {
    return this.formModelService.model;
  }

  ngOnInit() {
    this.metadataProcessorRegistry.initialize();
    const path = this.route.snapshot.paramMap.get('path');
    if (!path) return;

    this.formClient
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

  private handleFormResponse(form: FormResponse | null) {
    if (form == null) return;
    this.metadataLookup.initialize(form.modelType, form.modelMetadatas);
    this.viewLookup.initialize(form.views);
    this.formModelService.initialize();
    this._initialized = true;
    this.repositoryClient.create(form.modelType).subscribe((r) => this.handleRepositoryResponse(r));
  }

  private handleRepositoryResponse(resp: FileResponse) {
    resp.data.text().then((text) => {
      this.formModelService.patchValues([], JSON.parse(text));
    });
  }

  onSubmit() {
    // Do stuff
  }
}
