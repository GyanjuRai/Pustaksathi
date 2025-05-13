export interface TimeDiscount {
    discountId: number;
    bookId: number;
    discountPercent: number;
    saleStartDate: Date;
    saleEndDate: Date;
    onSale: boolean;
}

export interface TimeDiscountIdParam {
    discountId: number;
}

export interface TimeDiscountFilterOptionParam {
    bookId: number;
    isDeleted: boolean;
}

export interface Annoucement {
    annoucementId: number;
    title: string;
    message: string;
    imageUrl: string;
    startDate: Date;
    endDate: Date;
}

export interface AnnoucementIdParam {
    annoucementId: number;
}