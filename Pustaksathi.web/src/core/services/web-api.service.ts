import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConst } from '../../app/app.const';
import { catchError, delay, map, Observable, of, retry } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Injectable({
    providedIn: 'root'
})
export class WebApiService {

    private apiUrl: string;
    // accessToken : string = "";

    constructor(private http: HttpClient) {
        this.apiUrl = `${AppConst?.data?.apiBaseUrl}`;
    }


    get(url: string, param?: object, nestedParam = false, showLoader = false, refresh = true): Observable<any> {
        
        let params = {};
        if(nestedParam) {
            this.buildHttpParams(params, param, '');
        } else {
            params = param as HttpParams;
        }
        
        return this.http.get(`${this.apiUrl}${url}`, { params: params, withCredentials: true}).pipe(
            delay(100),
            retry(0),
            // map(response => retur)
        )
    }

    private buildHttpParams(params: any, data: any, currentPath: string) {
        Object.keys(data).forEach(key => {
          if (data[key] instanceof Object && !(data[key] instanceof Array)) {
            this.buildHttpParams(params, data[key], `${currentPath}${key}.`);
          } else {
            params[`${currentPath}${key}`] = data[key];
          }
        });
      }
    

    post(url: string, param?: object, showLoader = false): Observable<any> {
        
        // if (showLoader && blockui)-- I will implement block UI

        return this.http.post(`${this.apiUrl}${url}`, param as HttpParams).pipe(
            delay(100),
            retry(0),
            // map(response => )
        )

    }



    // private returnResponse(value: any, showLoader: boolean = true): any {

    //     if (showLoader && this.blockUI.isActive) {
    //       this.blockUI.reset();
    //     }
    
    //     return value;
    //   }
}