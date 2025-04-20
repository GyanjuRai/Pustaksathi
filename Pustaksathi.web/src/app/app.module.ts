import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { AppConst } from './app.const';
import { HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient } from '@angular/common/http';
import { AuthInceptor } from '../core/inceptor/auth-inceptor';
import { RouterModule } from '@angular/router';
import { appRoutes } from './app.routing';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes)
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
