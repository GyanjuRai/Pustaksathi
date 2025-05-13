import { Routes } from '@angular/router';

export const appRoutes: Routes = [
    {
        path: '',
        redirectTo: '',
        pathMatch: 'full'
    },
    {
        path: 'auth',
        loadChildren: () => import('./shared/shared.module').then(m => m.SharedModule),
    }
];