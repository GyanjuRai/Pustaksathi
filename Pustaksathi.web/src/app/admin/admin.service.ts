import { Injectable } from '@angular/core';
import { WebApiService } from '../../core/services/web-api.service';
import { Observable } from 'rxjs';
import { FlagResponse, GridResponse, ResponseModel } from '../../core/model/base.model';
import { Annoucement, TimeDiscount } from './admin.model';

@Injectable({
    providedIn: 'any'
})

export class AdminService {
    constructor(
        private api: WebApiService
    ) { }

    public discountSel(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<GridResponse<TimeDiscount>>> {
        return this.api.get("Admin/DiscountSel", param, true, showLoader, refresh);
    }

    public discountTsk(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Admin/DiscountTsk", param, showLoader);
    }

    public discountDelete(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Admin/DiscountDel", param, showLoader);
    }

    public annoucementSel(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<GridResponse<Annoucement>>> {
        return this.api.get("Admin/AnnoucementsSel", param, true, showLoader, refresh);
    }

    public getAnnoucement(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<Annoucement[]>> {
        return this.api.get("Admin/AnnoucementsGet", param, false, showLoader, refresh);
    }

    public annoucementTsk(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Admin/AnnoucementsTsk", param, showLoader);
    }

    public annoucementDelete(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Admin/AnnoucementsDel", param, showLoader);
    }
}