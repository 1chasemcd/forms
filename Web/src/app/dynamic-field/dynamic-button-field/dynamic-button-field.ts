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
import { CustomButtonComponent } from '../../field-resolution/custom-field-registration';
import { RecalculateEventService } from '../../recalculate-event-service/recalculate-event-service';
import { applyPropertyOrConstant, getMetadata } from '../../utils/api-utils';
import { FormGroup } from '@angular/forms';
import { FieldDefinition, MetadataType, RecalculateEvent } from '../../api/api.g';
import { CustomFieldService } from '../../field-resolution/custom-field-service';

@Component({
  selector: 'app-dynamic-button-field',
  template: '<ng-container #container></ng-container>',
  providers: [RecalculateEventService],
})
export class DynamicButtonField implements OnInit, AfterViewInit {
  readonly label = input.required<string>();
  readonly field = input.required<FieldDefinition>();
  readonly modelFormGroup = input.required<FormGroup>();
  readonly enabled = signal(true);

  private injector = inject(Injector);
  private readonly customFieldService = inject(CustomFieldService);
  private readonly recalculateEventService = inject(RecalculateEventService);

  @ViewChild('container', { read: ViewContainerRef })
  vcr!: ViewContainerRef;

  buttonComponent = computed(() => {
    return this.customFieldService.getField<CustomButtonComponent>(this.field().Type);
  });

  ngOnInit(): void {
    applyPropertyOrConstant(
      getMetadata(this.field(), MetadataType.Enabled),
      this.modelFormGroup(),
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
    if (recalc) this.recalculateEventService.runRecalculate(this.modelFormGroup(), recalc);
  };
}
