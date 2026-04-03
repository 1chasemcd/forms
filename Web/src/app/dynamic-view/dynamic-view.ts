import { Component, computed, input, OnInit, signal } from '@angular/core';
import { widthToCss } from '../utils/width-utils';
import { DynamicField } from '../dynamic-field/dynamic-field/dynamic-field';
import { FormGroup } from '@angular/forms';
import { applyPropertyOrConstant } from '../utils/api-utils';
import { SubpropertyGridViewComponent } from '../grid/subproperty-grid-view/subproperty-grid-view';
import { BaseViewDefinition } from '../api/api.g';

@Component({
  selector: 'app-dynamic-view',
  host: {
    '[class]': 'width() + " grid grid-cols-12 gap-4 content-start"',
  },
  imports: [DynamicField, SubpropertyGridViewComponent],
  templateUrl: './dynamic-view.html',
})
export class DynamicView implements OnInit {
  readonly formView = input.required<BaseViewDefinition>();
  readonly modelFormGroup = input.required<FormGroup>();
  title = signal('');

  ngOnInit(): void {
    applyPropertyOrConstant(this.formView().Title, this.modelFormGroup(), this.title.set);
  }

  readonly combinedViews = computed(() => {
    const view = this.formView();
    return view.$type === 'combinedview' ? view.Views : null;
  });

  readonly fieldView = computed(() => {
    const view = this.formView();
    return view.$type === 'fieldview' ? view : null;
  });
  readonly subpropertyGridView = computed(() => {
    const view = this.formView();
    return view.$type == 'subpropertygridview' ? view : null;
  });
  readonly width = computed(() => widthToCss(this.formView().Width));
}
