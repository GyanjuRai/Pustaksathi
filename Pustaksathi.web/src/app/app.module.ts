import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { AppConst } from './app.const';
import { HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient } from '@angular/common/http';
import { AuthInceptor } from '../core/inceptor/auth-inceptor';
import { RouterModule } from '@angular/router';
import { appRoutes } from './app.routing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { SnackbarModule } from './shared/components/snackbar/snackbar.module';
import { SharedModule } from './shared/shared.module';
import { HomeComponent } from './home/home.component';
import { CatalogeComponent } from './cataloge/cataloge.component';
import {MatPaginatorModule} from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { BookDetailComponent } from './book-detail/book-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CatalogeComponent,
    BookDetailComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    SnackbarModule,
    SharedModule,
    MatPaginatorModule,
    FormsModule,
  ],
  providers: [
    AppConst,
    {
      provide: APP_INITIALIZER,
      useFactory: (appConst: AppConst) => () => appConst.load(),
      deps: [AppConst],
      multi: true
    },
    provideHttpClient(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
