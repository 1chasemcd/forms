import { Component, inject, input, OnInit, output, signal } from '@angular/core';
import { FieldDefinition } from '../../api/api.g';
import { FormContext } from '../../dynamic-form/form-context';
import { FormFieldEnablementService } from '../../form-processor/form-field-enablement-service';

@Component({
  selector: 'app-button',
  templateUrl: './button.html',
})
export class Button implements OnInit {
  readonly enablementService = inject(FormFieldEnablementService);

  readonly label = input.required<string>();
  readonly field = input.required<FieldDefinition>();
  readonly formContext = input.required<FormContext>();
  readonly recalculateEvent = output();

  readonly enabled = signal(true);

  ngOnInit(): void {
    this.enablementService.enablementOf(this.field()).subscribe(this.enabled.set);
  }
}
