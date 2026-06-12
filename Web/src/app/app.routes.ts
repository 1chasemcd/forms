import { Routes } from '@angular/router';
import { FormRoot } from './form-root/form-root';

export const routes: Routes = [
  {
    path: 'form',
    children: [
      {
        path: '**',
        component: FormRoot,
      },
    ],
  },
];
