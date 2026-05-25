import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { widthToCss } from '../utils/width-utils';
import { DynamicControl } from '../dynamic-control/dynamic-control/dynamic-control';
import { applyPropertyOrConstant } from '../utils/api-utils';
import { SubpropertyGridViewComponent } from '../grid/subproperty-grid-view/subproperty-grid-view';
import { NgClass } from '@angular/common';
import { ViewLookupService } from '../dynamic-form/view-lookup-service';
import { FormModelService } from '../dynamic-form/form-model-service';
import { ControlPath } from '../utils/form-utils';

@Component({
  selector: 'app-dynamic-view',
  host: {
    '[class]': 'classes()',
  },
  imports: [DynamicControl, SubpropertyGridViewComponent, NgClass],
  templateUrl: './dynamic-view.html',
})
export class DynamicView implements OnInit {
  private readonly viewLookup = inject(ViewLookupService);
  private readonly formModelService = inject(FormModelService);

  readonly viewId = input.required<number>();
  readonly modelPath = input.required<ControlPath>();
  readonly alreadyInUnifiedView = input(false);

  readonly view = computed(() => this.viewLookup.lookupById(this.viewId()));
  title = signal('');

  ngOnInit(): void {
    applyPropertyOrConstant(this.view()?.title, this.formModelService.model, this.title.set);
  }

  readonly classes = computed(() =>
    [
      this.width(),
      this.shouldStyleUnifiedView()
        ? 'bg-white rounded-lg shadow'
        : 'grid grid-cols-12 gap-4 content-start',
    ].join(' '),
  );

  readonly combinedView = computed(() => {
    const view = this.view();
    return view?.$type === 'combinedView' ? view : null;
  });

  readonly controlView = computed(() => {
    const view = this.view();
    return view?.$type === 'controlView' ? view : null;
  });
  readonly subpropertyGridView = computed(() => {
    const view = this.view();
    return view?.$type == 'subPropertyGridView' ? view : null;
  });
  readonly width = computed(() => widthToCss(this.view()?.width));

  readonly shouldStyleUnifiedView = computed(() => {
    const view = this.view();
    return !this.alreadyInUnifiedView() && (view?.$type !== 'combinedView' || view?.unify);
  });
}
