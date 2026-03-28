import {
  AfterViewInit,
  Component,
  computed,
  effect,
  inject,
  Injector,
  input,
  OnInit,
  signal,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { ButtonField } from '../../api/api.g';
import { CUSTOM_FIELDS } from '../../field-resolution/custom-field-provider';
import { CustomButtonComponent } from '../../field-resolution/custom-field-registration';
import { RecalculateEventService } from '../../recalculate-event-service/recalculate-event-service';
import { applyPropertyOrConstant } from '../../utils/api-utils';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-dynamic-button-field',
  template: '<ng-container #container></ng-container>',
  providers: [RecalculateEventService],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicButtonField implements OnInit, AfterViewInit {
  readonly button = input.required<ButtonField>();
  readonly label = signal('');
  readonly disabled = signal(false);
  private registry = inject(CUSTOM_FIELDS);
  private injector = inject(Injector);
  private recalculateEventService = inject(RecalculateEventService);
  private parentForm = inject(ControlContainer) as FormGroupDirective;
  @ViewChild('container', { read: ViewContainerRef })
  vcr!: ViewContainerRef;

  buttonComponent = computed(() => {
    return this.registry.find((r) => r.type === 'buttonfield')?.component;
  });

  ngOnInit(): void {
    applyPropertyOrConstant(this.button().Label, this.parentForm.control, this.label.set);
    applyPropertyOrConstant(this.button().Disabled, this.parentForm.control, this.disabled.set);
  }

  ngAfterViewInit() {
    const comp = this.buttonComponent();
    if (!comp) return;
    this.load(comp);
  }

  load(comp: Type<CustomButtonComponent>) {
    this.vcr.clear();
    const ref = this.vcr.createComponent(comp);

    effect(() => ref.setInput('label', this.label()), { injector: this.injector });
    effect(() => ref.setInput('disabled', this.disabled()), { injector: this.injector });

    ref.instance.clicked.subscribe(() => {
      this.onClick();
    });
  }

  onClick = () => {
    this.recalculateEventService.runRecalculate(
      this.parentForm.control,
      this.button().RecalculateEvent,
    );
  };
}
