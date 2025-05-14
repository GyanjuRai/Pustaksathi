export interface Orders {
    orderId: number;
    userId: number;
    orderDate: Date;
    status: string;
    claimCode: string;
    isCancelled: boolean;
    loyalityDiscount: boolean;
    quantityDiscount: boolean;
    totalAmount: number;
    createdAt: Date;
    modifiedAt: Date;
    orderItems: OrderItems[];
}

export interface OrderItems {
    orderItemId: number;
    orderId: number;
    bookId: number;
    bookTitle: string;
    quantity: number;
    unitPrice: number;
}

export interface whiteList {
    whiteListId: number;
    userId: number;
    addedAt: Date;
    whiteListItems: WhiteListItems[];
}

export interface WhiteListItems {
    whiteListItemId: number;
    whiteListId: number;
    bookId: number;
    addedAt: Date;
}

export interface Cart {
    cartId: number;
    userId: number;
    createdAt: Date;
    cartItems: CartItems[];
}

export interface CartItems {
    cartItemId: number;
    cartId: number;
    bookId: number;
    bookTitle: string;
    quantity: number;
    totalPrice: number;
    createdAt: Date;
    modifiedAt: Date;
}

export interface OrderIdParam {
    orderId: number;
}

export interface OrderClaimCodeParam {
    claimCode: string;
}

export interface WhiteListItemIdParam {
    whiteListItemId: number;
}

export interface CartItemsIdParam {
    cartItemId: number;
}

export interface WhiteListTskParam {
    userId: number;
    whiteListItems: WhiteListItems[];
}

export interface CartTskParam {
    userId: number;
    cartItems: CartItems;
}

