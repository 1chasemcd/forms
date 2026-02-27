import { Component, computed, input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AppDataView } from '../data-view/data-view';
import { BaseView } from '../../api/api.g';

@Component({
  selector: 'app-dynamic-view',
  imports: [AppDataView],
  templateUrl: './dynamic-view.html',
})
export class DynamicView {
  readonly group = input<FormGroup>();
  readonly formView = input<BaseView>();

  readonly combinedViews = computed(() => {
    const view = this.formView();
    if (view?.$type == 'combinedview') return view.views;
    return undefined;
  });

  readonly dataView = computed(() => {
    const view = this.formView();
    if (view?.$type == 'dataview') return view;
    return undefined;
  });
}
