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

@Component({
  selector: 'app-dynamic-button-field',
  template: '<ng-container #container></ng-container>',
})
export class DynamicButtonField implements OnInit, AfterViewInit {
  readonly button = input.required<ButtonField>();
  readonly model = input.required<FormModel>();
  readonly label = signal('');
  readonly disabled = signal(false);
  private registry = inject(CUSTOM_FIELDS);
  private injector = inject(Injector);
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

    effect(
      () => {
        ref.setInput('label', this.label());
        ref.setInput('disabled', this.disabled());
      },
      { injector: this.injector },
    );

    ref.instance.clicked.subscribe(() => {
      this.onClick();
    });
  }

  onClick = () => {
    console.log('Button clicked, perform action');
    console.log(this.model());
  };
}
