import { Component, inject, OnInit, signal } from '@angular/core';
import { FileResponse, FormClient, FormResponse, RepositoryClient } from '../api/api.g';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { catchError, of } from 'rxjs';
import { MetadataLookupService } from '../metadata/metadata-lookup-service';
import { MetadataProcessorRegistryService } from '../metadata/metadata-processor-registry-service';
import { ViewLookupService } from '../form-services/view-lookup-service';
import { FormStackModel } from '../dynamic-form/form-stack-model';
import { ControlPath } from '../utils/form-utils';
import { DynamicForm } from '../dynamic-form/dynamic-form';
import { Spinner } from '../components/spinner/spinner';
import { Card } from '../components/card/card';

@Component({
  selector: 'app-form-root',
  imports: [ReactiveFormsModule, DynamicForm, Spinner, Card],
  templateUrl: './form-root.html',
  providers: [MetadataLookupService, MetadataProcessorRegistryService, ViewLookupService],
  host: {
    class: 'flex-1 flex justify-center',
  },
})
export class FormRoot implements OnInit {
  private readonly formClient = inject(FormClient);
  private readonly repositoryClient = inject(RepositoryClient);
  private readonly route = inject(ActivatedRoute);
  private readonly metadataLookup = inject(MetadataLookupService);
  private readonly viewLookup = inject(ViewLookupService);
  private readonly formStack: FormStackModel[] = [];

  readonly currentForm = signal<FormStackModel | undefined>(undefined);
  readonly notFound = signal(false);
  readonly error = signal(false);

  ngOnInit() {
    this.route.queryParamMap.subscribe((m) => this.updateParams(m));

    const path = this.route.snapshot.url.join('/');
    if (!path) return;

    this.formClient
      .getForm(path)
      .pipe(
        catchError((error) => {
          if (error.status == 404) {
            this.handleFormPathNotFound();
            return of(null);
          }
          this.handleError();
          return of(null);
        }),
      )
      .subscribe((f) => this.handleFormResponse(f));
  }

  private updateParams(paramMap: ParamMap) {
    console.log(paramMap);
  }

  private addToFormStack(viewId: number, modelPath: ControlPath) {
    const prevModel = structuredClone(this.formStack.at(-1)?.model ?? {});
    this.formStack.push({
      model: prevModel,
      viewId: viewId,
      modelPathRoot: modelPath,
    });
    this.currentForm.set(this.formStack.at(-1));
  }

  private setCurrentModel(model: Record<string, unknown>) {
    const current = this.formStack.at(-1);
    if (!current) return;
    current.model = model;
    this.currentForm.set(current);
  }

  private handleFormPathNotFound() {
    this.notFound.set(true);
  }
  private handleError() {
    this.error.set(true);
  }

  private handleFormResponse(form: FormResponse | null) {
    if (form == null) return;
    this.metadataLookup.initialize(form.modelType, form.modelMetadatas);
    this.viewLookup.initialize(form.views);
    this.addToFormStack(0, []);
    this.repositoryClient.create(form.modelType).subscribe((r) => this.handleRepositoryResponse(r));
  }

  private handleRepositoryResponse(resp: FileResponse) {
    resp.data.text().then((text) => {
      this.setCurrentModel(JSON.parse(text));
    });
  }
}
