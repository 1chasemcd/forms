import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { DynamicField } from '../dynamic-control/dynamic-field/dynamic-field';
import { SubPropertyTableViewComponent } from '../table/subproperty-table-view/subproperty-table-view';
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
  imports: [DynamicField, SubPropertyTableViewComponent, NgClass],
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

  readonly fieldView = computed(() => {
    const view = this.view();
    return view?.$type === 'fieldView' ? view : null;
  });
  readonly subpropertyTableView = computed(() => {
    const view = this.view();
    return view?.$type == 'subPropertyTableView' ? view : null;
  });
  readonly width = computed(() => widthToCss(this.view()?.width));
}
