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
import { CUSTOM_FIELDS } from '../../field-resolution/custom-field-provider';
import { CustomButtonComponent } from '../../field-resolution/custom-field-registration';
import { RecalculateEventService } from '../../recalculate-event-service/recalculate-event-service';
import { applyPropertyOrConstant, getMetadata } from '../../utils/api-utils';
import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { FieldDefinition, FieldType, MetadataType, RecalculateEvent } from '../../api/api.g';

@Component({
  selector: 'app-dynamic-button-field',
  template: '<ng-container #container></ng-container>',
  providers: [RecalculateEventService],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicButtonField implements OnInit, AfterViewInit {
  readonly field = input.required<FieldDefinition>();
  readonly label = signal('');
  readonly enabled = signal(true);
  private registry = inject(CUSTOM_FIELDS);
  private injector = inject(Injector);
  private recalculateEventService = inject(RecalculateEventService);
  private parentForm = inject(ControlContainer) as FormGroupDirective;
  @ViewChild('container', { read: ViewContainerRef })
  vcr!: ViewContainerRef;

  buttonComponent = computed(() => {
    return this.registry.find((r) => r.type === FieldType.Button)
      ?.component as Type<CustomButtonComponent>;
  });

  ngOnInit(): void {
    applyPropertyOrConstant(
      getMetadata(this.field(), MetadataType.Label),
      this.parentForm.control,
      this.label.set,
    );
    applyPropertyOrConstant(
      getMetadata(this.field(), MetadataType.Enabled),
      this.parentForm.control,
      this.enabled.set,
    );
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
    effect(() => ref.setInput('disabled', this.enabled()), { injector: this.injector });

    ref.instance.clicked.subscribe(() => {
      this.onClick();
    });
  }

  onClick = () => {
    const recalc = getMetadata<RecalculateEvent>(this.field(), MetadataType.RecalculateEvent);
    if (recalc) this.recalculateEventService.runRecalculate(this.parentForm.control, recalc);
  };
}
