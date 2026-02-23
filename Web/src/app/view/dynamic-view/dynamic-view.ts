import { Component, computed, input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormDataView, FormView } from '../../api/types';
import { DataView } from '../data-view/data-view';

@Component({
  selector: 'app-dynamic-view',
  imports: [DataView],
  templateUrl: './dynamic-view.html',
})
export class DynamicView {
  readonly group = input<FormGroup>();
  readonly formView = input<FormView>();

  readonly combinedViews = computed(() => {
    const view = this.formView();
    if (view?.$type == 'combinedview')
      return view.views?.map((v) => v as FormView);
    return undefined;
  });

  readonly dataView = computed(() => {
    if (this.formView()?.$type == 'dataview')
      return this.formView() as FormDataView;
    return undefined;
  });
}
