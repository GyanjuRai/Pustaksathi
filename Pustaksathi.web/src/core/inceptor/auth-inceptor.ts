import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Observable, tap } from 'rxjs';
import { AppConst } from '../../app/app.const';

@Injectable({
    providedIn: 'root'
})

export class AuthInceptor implements HttpInterceptor {

    constructor(
        private auth: AuthService
    ) {

    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const authReq = this.getRequestWithHeaders(req);
        return this.sendRequest(authReq, next);
    }

    sendRequest(
        req: HttpRequest<any>,
        next: HttpHandler
      ): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
          
        );
      }

    getRequestWithHeaders(req: HttpRequest<any>): any {
        let headers = req.headers;
        const url = req.url.toLowerCase();
        const isLoginUrl = req.url.includes("/login");
        const isRefereshTokenRequst = req.url.includes("/refreshtoken");

        if(isLoginUrl ? false : isRefereshTokenRequst ? false : true) {
            const token = this.auth.getLocalStorage("token") ?? '';
            if(token != ''){
                headers = headers.set('Authorization', `Bearer ${token}`);
            }
        }

        let origin = AppConst.data.webUrl ?? '*';
        headers = headers.set('Access-Control-Allow-Origin', origin);
        headers = headers.set('Access-Control-Allow-Headers', 'Origin, Authorization, Content-Type');
        headers = headers.set('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
        headers = headers.set('Accept', 'application/json');
        const contentType = 'application/json; charset=utf-8;'
        headers = headers.set('Content-Type', contentType);

        if (headers.get('Content-Type') == 'angular/auto') {
        headers = headers.delete('Content-Type');
        headers = headers.set('Content-Type', contentType);
        }

        return req.clone({ headers });
    }
}