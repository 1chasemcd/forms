import { DataSource } from '@angular/cdk/table';
import { map, Observable, of, startWith } from 'rxjs';
import { ControlPath } from '../../utils/form-utils';
import { FormStackService } from '../../form/form-services/form-stack-service';
import { FormArray, FormGroup } from '@angular/forms';

export class SubpropertyTableDataSource extends DataSource<FormGroup> {
  private readonly formArray: FormArray | null;

  constructor(
    path: ControlPath,
    private readonly formStack: FormStackService,
  ) {
    super();
    this.formArray = this.formStack.activeModel.get<FormArray>(path);
  }
  override connect(): Observable<readonly FormGroup[]> {
    return (
      this.formArray?.valueChanges.pipe(
        startWith([]),
        map(() => this.formArray?.controls as FormGroup[]),
      ) ?? of([])
    );
  }

  override disconnect(): void {
    return;
  }
}
