import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'fundos', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./features/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'fundos/novo',
    canActivate: [authGuard],
    loadComponent: () => import('./features/fundos/fundo-form.component').then(m => m.FundoFormComponent)
  },
  {
    path: 'fundos/:codigo/editar',
    canActivate: [authGuard],
    loadComponent: () => import('./features/fundos/fundo-form.component').then(m => m.FundoFormComponent)
  },
  {
    path: 'fundos',
    canActivate: [authGuard],
    loadComponent: () => import('./features/fundos/fundos-list.component').then(m => m.FundosListComponent)
  },
  { path: '**', redirectTo: '/fundos' }
];
