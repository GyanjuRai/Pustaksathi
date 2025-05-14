export interface loginResponse {
    userId: number;
    fullName: string;
    email: string;
    role: string;
    token: string;
    refreshToken: string;
}

export interface user {
    userId: number;
    fullName: string;
    email: string;
    passwordHash: string;
    roleId: number;
    isDiscountApplied : boolean;
    createdAt: Date;
    modifiedAt: Date;
}

export interface userLoginParam {
    email: string;
    passwordHash: string;
}

export interface userInfoResponse {
    userId: number;
    fullName?: string;
    email?: string;
    role?: string;
    isDiscountApplied?: boolean;
    whiteListId?: number;
    cartId?: number;
}

export interface userIdParm {
    userId: number;
}