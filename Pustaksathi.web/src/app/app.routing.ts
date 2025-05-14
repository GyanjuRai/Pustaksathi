import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CatalogeComponent } from './cataloge/cataloge.component';
import { BookDetailComponent } from './book-detail/book-detail.component';

export const appRoutes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        component: HomeComponent
    },
    {
        path: 'cataloge',
        component: CatalogeComponent
    },
    {
        path: 'product:id',
        component: BookDetailComponent
    },
    {
        path: 'auth',
        loadChildren: () => import('./shared/shared.module').then(m => m.SharedModule),
    },
    {
        path: '**',
        redirectTo: 'home',
        pathMatch: 'full'
    }
];