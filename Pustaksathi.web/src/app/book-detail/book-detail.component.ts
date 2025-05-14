import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { BooksService } from '../../core/services/books.service';
import { ResponseModel } from '../../core/model/base.model';
import { BookIdParam, BooksDetails } from '../../core/model/books.model';
import { EnumResponse } from '../../core/enum/base.enum';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-book-detail',
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.css'
})
export class BookDetailComponent implements OnInit, OnDestroy {
    private _unSubscribeAll: Subject<any>;
    bookId: number  = 0;
    bookDetail = {} as BooksDetails;
  
    constructor(
      private bs: BooksService,
      private route: ActivatedRoute,
    )
    {
      this._unSubscribeAll = new Subject();
      
    }

    ngOnInit(): void {
      this.route.paramMap.subscribe(params => {
      var paramProductId = params.get('id') ?? "0";
      this.bookId = parseInt(paramProductId);
    });
    }

    getBookDetail() {
      const param = {
        bookId :this.bookId
      } as BookIdParam;
      this.bs.getBookDetails()
      .pipe(takeUntil(this._unSubscribeAll))
      .subscribe((response: ResponseModel<BooksDetails>) => {
        if(response != null && response.type === EnumResponse.success){
          this.bookDetail = response.data!;
        }
      })
    }
    ngOnDestroy(): void {
      this._unSubscribeAll.next(null);
      this._unSubscribeAll.complete();
    }
}
