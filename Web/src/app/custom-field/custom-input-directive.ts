import { computed, Directive, ElementRef, inject, OnInit, signal } from '@angular/core';
import { CustomInputContainer } from './custom-input-container/custom-input-container';
import { NgControl, Validators } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, of, startWith } from 'rxjs';

@Directive({
  selector: 'input[appCustomInputDirective], textarea[appCustomInputDirective]',
  host: {
    '[id]': 'inputId',
    class: 'w-full bg-transparent text-base text-black outline-none disabled:text-gray-500',
  },
})
export class CustomInputDirective implements OnInit {
  private readonly _el = inject<ElementRef<HTMLElement>>(ElementRef<HTMLElement>);
  private readonly _container = inject(CustomInputContainer, { optional: true });
  private readonly _control = inject(NgControl, { optional: true });
  private readonly _nativeRequired = signal(false);
  private readonly _controlRequired = toSignal(
    this._control?.control?.statusChanges.pipe(
      startWith(null),
      map(() => this._control?.control?.hasValidator(Validators.required) ?? false),
    ) ?? of(false),
    { initialValue: false },
  );
  readonly isRequired = computed(() => {
    return this._controlRequired() || this._nativeRequired();
  });

  private static _nextId = 0;
  readonly inputId = `input-${CustomInputDirective._nextId++}`;

  ngOnInit() {
    const el = this._el.nativeElement;

    const update = () => {
      this._nativeRequired.set(el.hasAttribute('required'));
    };

    update();

    const observer = new MutationObserver(update);
    observer.observe(el, { attributes: true, attributeFilter: ['required'] });

    this._container?.registerInput(this.inputId, this.isRequired);
  }
}
