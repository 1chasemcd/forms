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
import { FormModel } from '../../dynamic-form/form-model';
import { CUSTOM_FIELDS } from '../../field-resolution/custom-field-provider';
import { CustomButtonComponent } from '../../field-resolution/custom-field-registration';
import { RecalculateEventService } from '../../recalculate-event-service/recalculate-event-service';

@Component({
  selector: 'app-dynamic-button-field',
  template: '<ng-container #container></ng-container>',
  providers: [RecalculateEventService],
})
export class DynamicButtonField implements OnInit, AfterViewInit {
  readonly button = input.required<ButtonField>();
  readonly model = input.required<FormModel>();
  readonly label = signal('');
  readonly disabled = signal(false);
  private registry = inject(CUSTOM_FIELDS);
  private injector = inject(Injector);
  private recalculateEventService = inject(RecalculateEventService);
  @ViewChild('container', { read: ViewContainerRef })
  vcr!: ViewContainerRef;

  buttonComponent = computed(() => {
    return this.registry.find((r) => r.type === 'buttonfield')?.component;
  });

  ngOnInit(): void {
    this.model().registerPocDependency(this.button().Label, this.label.set);
    this.model().registerPocDependency(this.button().Disabled, this.disabled.set);
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
    this.recalculateEventService.runRecalculate(this.model(), this.button().RecalculateEvent);
  };
}
