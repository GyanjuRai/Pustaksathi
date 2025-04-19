import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConst } from '../../app/app.const';
import { catchError, delay, map, Observable, of, retry } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Injectable({
    providedIn: 'root'
})
export class WebApiService {

    @BlockUI() blockUi!: NgBlockUI;
    apiUrl: string;

    constructor(private http: HttpClient) {
        this.apiUrl = `${AppConst?.data?.apirBaseUrl}${AppConst?.data?.apiSegment}`;
    }

    get(url: string, param?: object, nestedParam = false, showLoader = false, refresh = true): Observable<any> {
        
        if(showLoader && !this.blockUi.isActive) {
            this.blockUi.start();
        }

        let params = {};
        if(nestedParam) {
            this.buildHtppParams(params, param, '');
        } else
        {
            params = param as HttpParams;
        }

        return this.http.get(`${this.apiUrl}${url}`, { params: params, withCredentials: true }).pipe(
            delay(100),
            retry(0),
            map(response => this.returnResponse(response, showLoader)),
            catchError((error) => {

                if(this.blockUi.isActive) this.blockUi.stop();

                return of(error);
            })
        );
    }

    private buildHtppParams(params: any, data: any, currentPath: string) {
        Object.keys(data).forEach(key => {
            if(data[key] instanceof Object && !(data[key] instanceof Array)) {
                this.buildHtppParams(params, data[key], `${currentPath}${key}.`);
            } else {
                params[`${currentPath}${key}}`] = data[key];
            }
        });
    }

    post(url: string, param?: object, showLoader = false): Observable<any> {

        if(showLoader && !this.blockUi.isActive) this.blockUi.start();

        return this.http.post(`${this.apiUrl}${url}`, param as HttpParams).pipe(
            delay(100),
            retry(0),
            map((response: any) => this.returnResponse(response, showLoader)),
            catchError((error) => {

                if(this.blockUi.isActive) this.blockUi.stop();

                return of(error);
            })
        );
    }

    private returnResponse(value: any, showLoader: boolean = true): any {

        if(showLoader && !this.blockUi.isActive) this.blockUi.reset();

        return value;
    }
}