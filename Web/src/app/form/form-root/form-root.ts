import { Component, inject, OnInit, signal } from '@angular/core';
import { FormClient, FormResponse, SwaggerException } from '../../api/api.g';
import { ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { catchError, of } from 'rxjs';
import { MetadataProcessorRegistryService } from '../../metadata/metadata-processor-registry-service';
import { ViewLookupService } from '../form-services/view-lookup-service';
import { DynamicForm } from '../dynamic-form/dynamic-form';
import { Spinner } from '../../components/spinner/spinner';
import { FormStackService } from '../form-services/form-stack-service';
import { MetadataLookupService } from '../../metadata/metadata-lookup';
import { DefaultMetadataProcessorsService } from '../../metadata/default-metadata-processors-service';
import { ServiceMethodService } from '../../service-method/service-method-service';

@Component({
  selector: 'app-form-root',
  imports: [ReactiveFormsModule, DynamicForm, Spinner],
  templateUrl: './form-root.html',
  providers: [
    MetadataProcessorRegistryService,
    DefaultMetadataProcessorsService,
    ViewLookupService,
    MetadataLookupService,
    FormStackService,
    ServiceMethodService,
    DefaultMetadataProcessorsService,
  ],
  host: {
    class: 'flex-1 flex justify-center',
  },
})
export class FormRoot implements OnInit {
  private readonly formClient = inject(FormClient);
  private readonly route = inject(ActivatedRoute);
  private readonly defaultMetadataProcessors = inject(DefaultMetadataProcessorsService);
  private readonly viewLookup = inject(ViewLookupService);
  private readonly metadataLookup = inject(MetadataLookupService);
  readonly formStack = inject(FormStackService);

  readonly notFound = signal(false);
  readonly error = signal(false);

  ngOnInit() {
    this.defaultMetadataProcessors.initialize();

    const path = this.route.snapshot.url.join('/');
    if (!path) return;

    this.formClient
      .getForm(path)
      .pipe(catchError((error) => of(this.handleError(error) ?? null)))
      .subscribe((f) => this.handleFormResponse(f));
  }

  private handleError(error: unknown) {
    if (error instanceof SwaggerException && error.status == 404) this.notFound.set(true);
    else this.error.set(true);
  }

  private handleFormResponse(form: FormResponse | null) {
    if (form == null) return;

    this.metadataLookup.initialize(form.modelMetadatas);
    this.viewLookup.initialize(form.views);
    this.formStack.pushRepository(0, form.modelType);
  }
}
