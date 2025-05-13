import { Injectable } from '@angular/core';
import { WebApiService } from './web-api.service';
import { Observable } from 'rxjs';
import { ResponseModel } from '../model/base.model';
import { loginResponse, user, userInfoResponse } from '../../app/shared/auth/auth.model';

@Injectable({
    providedIn: 'root'
})

export class AccountService 
{
    constructor(private api: WebApiService){

    }

    login(param?: object, showLoader = false): Observable<ResponseModel<loginResponse>> {
        return this.api.post("Account/Login", param, showLoader);
    }

    register(param?: object, showLoader = false): Observable<ResponseModel<user>> {
        return this.api.post("Account/UserTsk", param, showLoader);
    }

    getUserInfo(param?: object, showLoader = false): Observable<ResponseModel<userInfoResponse>> {
        return this.api.get("Account/GetUserInfo", param, false, showLoader);
    }
}