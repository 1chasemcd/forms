import { Component, computed, input } from '@angular/core';
import { BaseView } from '../../api/api.g';
import { computedPropertyOrConstant, widthToCss, FormModel } from '../../utils/api-model-utils';
import { DynamicField } from '../../field/dynamic-field/dynamic-field';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-dynamic-view',
  host: {
    '[class]': 'width() + " grid grid-cols-12 gap-4 content-start"',
  },
  imports: [DynamicField],
  templateUrl: './dynamic-view.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicView {
  readonly formView = input<BaseView>();
  readonly model = input<FormModel>();

  readonly title = computedPropertyOrConstant(
    computed(() => this.formView()?.Title),
    this.model,
  );

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

  readonly width = computed(() => widthToCss(this.formView()?.Width));
}
