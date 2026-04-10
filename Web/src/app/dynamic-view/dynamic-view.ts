import { Component, computed, input, OnInit, signal } from '@angular/core';
import { widthToCss } from '../utils/width-utils';
import { DynamicField } from '../dynamic-field/dynamic-field/dynamic-field';
import { FormGroup } from '@angular/forms';
import { applyPropertyOrConstant } from '../utils/api-utils';
import { SubpropertyGridViewComponent } from '../grid/subproperty-grid-view/subproperty-grid-view';
import { BaseViewDefinition, FormDefinition } from '../api/api.g';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-dynamic-view',
  host: {
    '[class]': 'classes',
  },
  imports: [DynamicField, SubpropertyGridViewComponent, NgClass],
  templateUrl: './dynamic-view.html',
})
export class DynamicView implements OnInit {
  readonly formDefinition = input.required<FormDefinition>();
  readonly formView = input.required<BaseViewDefinition>();
  readonly modelFormGroup = input.required<FormGroup>();
  readonly alreadyInUnifiedView = input(false);
  title = signal('');

  ngOnInit(): void {
    applyPropertyOrConstant(this.formView().title, this.modelFormGroup(), this.title.set);
  }

  get classes(): string {
    return [
      this.width(),
      this.shouldStyleUnifiedView()
        ? 'bg-white rounded-lg shadow'
        : 'grid grid-cols-12 gap-4 content-start',
    ].join(' ');
  }

  readonly combinedViews = computed(() => {
    const view = this.formView();
    return view.$type === 'combinedview' ? view.views : null;
  });

  readonly fieldView = computed(() => {
    const view = this.formView();
    return view.$type === 'fieldview' ? view : null;
  });
  readonly subpropertyGridView = computed(() => {
    const view = this.formView();
    return view.$type == 'subpropertygridview' ? view : null;
  });
  readonly width = computed(() => widthToCss(this.formView().width));

  readonly shouldStyleUnifiedView = computed(() => {
    const view = this.formView();
    return !this.alreadyInUnifiedView() && (view.$type !== 'combinedview' || view.unify);
  });
}
