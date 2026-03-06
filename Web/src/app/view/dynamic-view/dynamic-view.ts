import { Component, computed, input } from '@angular/core';
import { AppDataView } from '../data-view/data-view';
import { BaseView } from '../../api/api.g';
import { FormModel } from '../../dynamic-form/form-model';

@Component({
  selector: 'app-dynamic-view',
  imports: [AppDataView],
  templateUrl: './dynamic-view.html',
})
export class DynamicView {
  readonly formView = input<BaseView>();
  readonly model = input<FormModel>();

  readonly combinedViews = computed(() => {
    const view = this.formView();
    if (view?.$type == 'combinedview') return view.Views;
    return undefined;
  });

  readonly dataView = computed(() => {
    const view = this.formView();
    if (view?.$type == 'dataview') return view;
    return undefined;
  });
}
