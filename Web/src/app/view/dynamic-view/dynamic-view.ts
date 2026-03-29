import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { BaseView } from '../../api/api.g';
import { widthToCss } from '../../utils/width-utils';
import { DynamicField } from '../../dynamic-field/dynamic-field/dynamic-field';
import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { applyPropertyOrConstant } from '../../utils/api-utils';
import { SubpropertyGridViewComponent } from '../../grid/subproperty-grid-view/subproperty-grid-view';

@Component({
  selector: 'app-dynamic-view',
  host: {
    '[class]': 'width() + " grid grid-cols-12 gap-4 content-start"',
  },
  imports: [DynamicField, SubpropertyGridViewComponent],
  templateUrl: './dynamic-view.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicView implements OnInit {
  readonly formView = input.required<BaseView>();
  title = signal('');
  private readonly parentForm = inject(ControlContainer) as FormGroupDirective;

  ngOnInit(): void {
    applyPropertyOrConstant(this.formView().Title, this.parentForm.control, this.title.set);
  }

  readonly combinedViews = computed(() => {
    const view = this.formView();
    return view.$type === 'combinedview' ? view.Views : null;
  });

  readonly dataView = computed(() => {
    const view = this.formView();
    return view.$type === 'dataview' ? view : null;
  });
  readonly subpropertyGridView = computed(() => {
    const view = this.formView();
    return view.$type == 'subpropertygridview' ? view : null;
  });
  readonly width = computed(() => widthToCss(this.formView().Width));
}
