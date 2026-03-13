import { Component, inject, OnInit } from '@angular/core';
import { FileResponse, FormClient, FormDefinition, RepositoryClient } from '../api/api.g';
import { FormControlService } from './form-control-service';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { catchError, of, throwError } from 'rxjs';
import { DynamicView } from '../view/dynamic-view/dynamic-view';
import { FormModel } from './form-model';

@Component({
  selector: 'app-dynamic-form',
  imports: [ReactiveFormsModule, DynamicView],
  templateUrl: './dynamic-form.html',
  providers: [FormClient, RepositoryClient, FormControlService],
})
export class DynamicForm implements OnInit {
  private readonly formClient = inject(FormClient);
  private readonly repositoryClient = inject(RepositoryClient);
  private readonly formControlService = inject(FormControlService);
  private readonly route = inject(ActivatedRoute);
  formDefinition?: FormDefinition;
  model: FormModel = new FormModel();
  formGroup?: FormGroup;

  ngOnInit() {
    const path = this.route.snapshot.paramMap.get('path');
    if (path == null) return;

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

  private handleFormResponse(form: FormDefinition | null) {
    if (form == null) return;
    this.formDefinition = form;
    this.formGroup = this.formControlService.createFromDefinition(form, this.model);
    if (form.Type)
      this.repositoryClient.create(form.Type).subscribe((r) => this.handleRepositoryResponse(r));
  }

  private handleRepositoryResponse(resp: FileResponse) {
    resp.data.text().then((text) => this.model.patch(JSON.parse(text)));
  }

  onSubmit() {
    // Do stuff
  }
}
