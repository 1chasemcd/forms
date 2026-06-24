import { Component, inject, input } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { DynamicView } from '../../dynamic-view/dynamic-view';
import { ControlPath } from '../../utils/form-utils';
import { FormStackService } from '../form-services/form-stack-service';

@Component({
  selector: 'app-dynamic-form',
  imports: [ReactiveFormsModule, DynamicView],
  templateUrl: './dynamic-form.html',
  providers: [],
  host: {
    class: 'min-[72rem]:max-w-6xl 2xl:max-w-3/4 w-full h-full p-4',
  },
})
export class DynamicForm {
  readonly formStack = inject(FormStackService);
  readonly viewId = input.required<number>();
  readonly path = input.required<ControlPath>();

  onSubmit() {
    // TODO
  }
}
