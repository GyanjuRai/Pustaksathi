import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { BooksService } from '../../core/services/books.service';
import { NotificationService } from '../../core/services/notification.service';
import { MvGridConfig } from '../../core/model/gridConfig.model';
import { GridResponse, ResponseModel } from '../../core/model/base.model';
import { BooksDetails } from '../../core/model/books.model';
import { EnumResponse } from '../../core/enum/base.enum';
import { AttributeCategoryParam, AttributeItem } from '../../core/model/attribute.model';
import { UtilityService } from '../../core/services/utility.service';

@Component({
  selector: 'app-cataloge',
  templateUrl: './cataloge.component.html',
  styleUrl: './cataloge.component.css'
})
export class CatalogeComponent implements OnInit, OnDestroy {
  private _unSubscribeAll: Subject<any>;
  languageAttributeItemList: AttributeItem[] = [];
  genreAttributeItemList: AttributeItem[] = [];
  formatAttributeItemList: AttributeItem[] = [];

  pageSizeOptions = [5, 10, 25, 50];
  currentPage = 1;
  totalPages = 1;

  

  gridConfig: MvGridConfig = {
    columns: [],
    dataSource: {
      data: [],
      totalRows: 0
    },
    loading: false,
    option: {
      searchText: '',
      filter: {
        languageAttributeItemList: [],
        genreAttributeItemList: [],
        formatAttributeItemList: []
      },
      offset: 0,
      pageSize: 10,
      sortBy: 'Title',
      sortOrder: 'ASC'
    }
  };

  constructor(
    private bs: BooksService,
    private ns: NotificationService,
    private us: UtilityService
  ) 
  {
    this._unSubscribeAll = new Subject();
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(){
    this.getBooks();
    this.getLanguageAttributeItemList();
    this.getGenreAttributeItemList();
    this.getFormatAttributeItemList();
  }

  getBooks(){
    this.gridConfig.loading = true;
    const param = {...this.gridConfig.option};
    this.bs.booksSel(param)
    .pipe(takeUntil(this._unSubscribeAll))
    .subscribe((response: ResponseModel<GridResponse<BooksDetails>>) => {
      if(response.data != null && response.type === EnumResponse.success){
        this.gridConfig.dataSource.data = response.data.data;
        this.gridConfig.dataSource.totalRows = response.data.totalRows;
      } else{
        this.gridConfig.dataSource.data = [];
        this.gridConfig.dataSource.totalRows = 0;
      }
    });

    this.gridConfig.loading = false;
    this.gridConfig = { ...this.gridConfig }; //refresh grid
  }

  getLanguageAttributeItemList() {
    const param ={
      categoryName: "Language"
    } as AttributeCategoryParam;
    this.us.getAttributeCategory(param)
    .pipe(takeUntil(this._unSubscribeAll))
    .subscribe((response: ResponseModel<AttributeItem[]>) => {
      if(response.data != null && response.type === EnumResponse.success){
        this.languageAttributeItemList = response.data;
      } else{
        this.languageAttributeItemList = [];
      }
    });
  }

  getGenreAttributeItemList() {
    const param ={
      categoryName: "Genre"
    } as AttributeCategoryParam;

    this.us.getAttributeCategory(param)
    .pipe(takeUntil(this._unSubscribeAll))
    .subscribe((response: ResponseModel<AttributeItem[]>) => {
      if(response.data != null && response.type === EnumResponse.success){
        this.genreAttributeItemList = response.data;
      } else{
        this.genreAttributeItemList = [];
      }
    });
  }

  getFormatAttributeItemList() {
    const param ={
      categoryName: "Format"
    } as AttributeCategoryParam;

    this.us.getAttributeCategory(param)
    .pipe(takeUntil(this._unSubscribeAll))
    .subscribe((response: ResponseModel<AttributeItem[]>) => {
      if(response.data != null && response.type === EnumResponse.success){
        this.formatAttributeItemList = response.data;
      } else{
        this.formatAttributeItemList = [];
      }
    });
  }

  onFilterChange(): void {
    this.getBooks();
}

resetFilters(): void {
    this.gridConfig.option.searchText = '';
    this.gridConfig.option.filter.languageAttributeItemList = [];
    this.gridConfig.option.filter.genreAttributeItemList = [];
    this.gridConfig.option.filter.formatAttributeItemList = [];

    this.getBooks();
}
  onPageChange(event: any): void {
    this.gridConfig.option.pageSize = event.pageSize;
    this.gridConfig.option.offset = event.pageIndex * event.pageSize;
    this.currentPage = event.pageIndex + 1;

    this.getBooks();
  }

  ngOnDestroy(): void {
    this._unSubscribeAll.next(null);
    this._unSubscribeAll.complete();
  }
}
