import { Routes } from '@angular/router';
import { EnrollmentComponent } from './enrollment/enrollment.component';
import { LispComponent } from './lisp/lisp.component';
import { ProviderComponent } from './provider/provider.component';

export const appRoutes: Routes = [
  {
    path: 'home',
    component: ProviderComponent,
  },
  {
    path: 'lisp',
    component: EnrollmentComponent,
  },
  {
    path: 'enrollment',
    component: EnrollmentComponent,
  },
  {
    path: '**',
    redirectTo: 'home',
    pathMatch: 'full',
  },
];
