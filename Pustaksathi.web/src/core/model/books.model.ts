export interface BooksDetails  {
    bookId: number;
    title: string;
    description: string;
    ISBN: string;
    price: number;
    inStock: number;
    publishedDate: Date;
    discountPercent: number;
    languageId: number[];
    genreId: number[];
    formatId: number[];
    awardId: number[];
    authorId: number;
    createdAt: Date;
    modifiedAt: Date;
}

export interface BookFilterOptionParam {
    languageAttributeItemList: number[];
    genreAttributeItemList: number[];
    formatAttributeItemList: number[];
    authorId: number;
}

export interface BookIdParam {
    bookId: number;
}

export interface Review {
    reviewId: number;
    bookId: number;
    userId: number;
    rating: number;
    reviewText: string;
    createdAt: Date;
    modifiedAt: Date;
    isDeleted: boolean;
}

export interface ReviewResponse {
    reviewId: number;
    bookId: number;
    userId: number;
    userName: string;
    rating: number;
    reviewText: string;
    createdAt: Date;
    modifiedAt: Date;
    isDeleted: boolean;
}

export interface ReviewIdParam {
    reviewId: number;
}

export interface CheckReviewParam {
    bookId: number;
    userId: number;
}