import { Injectable } from '@angular/core';
import { WebApiService } from './web-api.service';
import { Observable } from 'rxjs';
import { FlagResponse, GridResponse, ResponseModel } from '../model/base.model';
import { BooksDetails, ReviewResponse } from '../model/books.model';

@Injectable({
    providedIn: 'root'
})

export class BooksService {
    constructor(
        private api: WebApiService
    ) { }

    public booksSel(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<GridResponse<BooksDetails>>> {
        return this.api.get("Books/BooksSel", param, true, showLoader, refresh);
    }

    public getBookDetails(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<BooksDetails>> {
        return this.api.get("Books/GetBookDetails", param, false, showLoader, refresh);
    }

    public booksTsk(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Books/BooksTsk", param, showLoader);
    }

    public booksDelete(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Books/BookDel", param, showLoader);
    }

    public reviewItemsSel(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<ReviewResponse[]>> {
        return this.api.get("Books/ReviewItemsSel", param, false, showLoader, refresh);
    }

    public reviewCheck(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.get("Books/ReviewCheck", param, false, showLoader, refresh);
    }

    public reviewTsk(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Books/ReviewTsk", param, showLoader);
    }

    public reviewDelete(param?: object, showLoader = false): Observable<ResponseModel<FlagResponse>> {
        return this.api.post("Books/ReviewDel", param, showLoader);
    }
}