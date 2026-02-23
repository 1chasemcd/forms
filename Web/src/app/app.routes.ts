import { Routes } from '@angular/router';
import { DynamicForm } from './dynamic-form/dynamic-form';

export const routes: Routes = [
  {
    path: 'form/:path',
    component: DynamicForm,
  },
];
