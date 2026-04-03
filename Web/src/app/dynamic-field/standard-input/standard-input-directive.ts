import { computed, Directive, ElementRef, inject, OnInit, signal } from '@angular/core';
import { NgControl, Validators } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, of, startWith } from 'rxjs';
import { StandardInputWrapper } from './standard-input-wrapper';

@Directive({
  selector: 'input[appStandardInputDirective], textarea[appStandardInputDirective]',
  host: {
    '[id]': 'inputId',
    class: 'w-full bg-transparent text-base text-black outline-none disabled:text-gray-500',
  },
})
export class StandardInputDirective implements OnInit {
  private readonly element = inject<ElementRef<HTMLElement>>(ElementRef<HTMLElement>);
  private readonly container = inject(StandardInputWrapper, { optional: true });
  private readonly control = inject(NgControl, { optional: true });
  private readonly nativeRequired = signal(false);
  private readonly controlRequired = toSignal(
    this.control?.control?.statusChanges.pipe(
      startWith(null),
      map(() => this.control?.control?.hasValidator(Validators.required) ?? false),
    ) ?? of(false),
    { initialValue: false },
  );

  readonly isRequired = computed(() => {
    return this.controlRequired() || this.nativeRequired();
  });

  private static nextId = 0;
  readonly inputId = `input-${StandardInputDirective.nextId++}`;

  ngOnInit() {
    const el = this.element.nativeElement;

    const update = () => {
      this.nativeRequired.set(el.hasAttribute('required'));
    };

    update();

    const observer = new MutationObserver(update);
    observer.observe(el, { attributes: true, attributeFilter: ['required'] });

    this.container?.registerInput(this.inputId, this.isRequired);
  }
}
