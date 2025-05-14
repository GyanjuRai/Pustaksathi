export interface ResponseModel<T> {
    data?: T;
    message: string;
    type: string;
    exception?: ExceptionResponse[];
}

export interface ExceptionResponse {
    type: string;
    message: string;
    exceptionType: string;
}

export interface MvReqOptionParam<T> {
    filter?: T;
    pageSize: number;
    searchText?: string;
    offSet: number;
    sortBy?: string;
    sortOrder?: string;
    tabCategories?: string;
}

export interface GridResponse<T> {
    totalRows: number;
    data: T[];
}

export interface FlagResponse {
    isSuccess: boolean;
    message: string;
}

export interface AttributeItemResponse {
    attributeId: number;
    itemName: string;
    itemValue: string;
}

export interface LoginResponseModel {
    userId: number;
    fullName: string;
    email: string;
    role: string;
    token: string;
    refreshToken: string;
}