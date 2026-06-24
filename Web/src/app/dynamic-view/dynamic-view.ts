import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { DynamicControl } from '../dynamic-control/dynamic-control/dynamic-control';
import { SubpropertyGridViewComponent } from '../grid/subproperty-grid-view/subproperty-grid-view';
import { NgClass } from '@angular/common';
import { ViewLookupService } from '../form/form-services/view-lookup-service';
import { widthToCss } from '../utils/width-utils';
import { ControlPath } from '../utils/form-utils';
import { FormStackService } from '../form/form-services/form-stack-service';

@Component({
  selector: 'app-dynamic-view',
  host: {
    class: 'rounded-lg border-gray-200',
    '[class]': 'width()',
    '[class.border]': '!parentIsUnified()',
  },
  imports: [DynamicControl, SubpropertyGridViewComponent, NgClass],
  templateUrl: './dynamic-view.html',
})
export class DynamicView implements OnInit {
  private readonly viewLookup = inject(ViewLookupService);
  private readonly formStack = inject(FormStackService);

  readonly viewId = input.required<number>();
  readonly path = input.required<ControlPath>();
  readonly parentIsUnified = input(false);

  readonly view = computed(() => this.viewLookup.lookupById(this.viewId()));
  title = signal('');

  ngOnInit(): void {
    this.formStack.activeModel.valueRefAugmentor
      ?.getValue<string>(this.path(), this.view()?.title)
      ?.subscribe((t) => this.title.set(t));
  }

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
}
