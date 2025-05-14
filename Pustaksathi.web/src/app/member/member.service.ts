import { Injectable } from '@angular/core';
import { WebApiService } from '../../core/services/web-api.service';
import { Cart, Orders, whiteList } from './member.model';
import { FlagResponse, ResponseModel } from '../../core/model/base.model';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'any'
})

export class MemberService {
    constructor(
        private api: WebApiService
    ) { }

    public getOrdersByUser(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<Orders[]>> {
        return this.api.get("Member/GetOrdersByUserId", param, false, showLoader, refresh);
    }

    public orderTsk(param?: object, showLoader = false): Observable<ResponseModel<Orders>> {
        return this.api.post("Member/OrderTsk", param, showLoader);
    }

    public cancelOrder(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Member/CancelOrder", param, showLoader);
    }

    public whiteListSel(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<whiteList[]>> {
        return this.api.get("Member/WhiteListSel", param, false, showLoader, refresh);
    }

    public whiteListTsk(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Member/WhiteListTsk", param, showLoader);
    }

    public whiteListDelete(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Member/WhiteListItemDel", param, showLoader);
    }

    public cartSel(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<Cart[]>> {
        return this.api.get("Member/CartSel", param, false, showLoader, refresh);
    }

    public cartTsk(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Member/CartTsk", param, showLoader);
    }

    public cartDelete(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Member/CartItemDel", param, showLoader);
    }
}