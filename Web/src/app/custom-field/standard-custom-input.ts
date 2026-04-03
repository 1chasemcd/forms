// import { Component, input } from '@angular/core';
// import { CustomInputComponent } from '../field-resolution/custom-field-registration';
// import { FormControl, ReactiveFormsModule } from '@angular/forms';
// import { CustomInputContainer } from './custom-input-container/custom-input-container';
// import { CustomInputDirective } from '../dynamic-field/standard-input/standard-input-directive';

// @Component({ template: '' })
// class StandardCustomInput implements CustomInputComponent {
//   readonly label = input.required<string>();
//   readonly formControl = input.required<FormControl>();
//   readonly inputType: string = 'text';
// }

// export function createStandardCustomInput(type: string) {
//   @Component({
//     selector: 'app-custom-input',
//     imports: [ReactiveFormsModule, CustomInputContainer, CustomInputDirective],
//     template: `<app-custom-input-container [label]="label()">
//       <input [formControl]="formControl()" [type]="inputType" appCustomInputDirective />
//     </app-custom-input-container>`,
//   })
//   class GeneratedCustomInput extends StandardCustomInput {
//     override readonly inputType = type;
//   }

//   return GeneratedCustomInput;
// }
