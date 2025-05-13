export interface AttributeItem {
    attributeItemId: number;
    attributeCategoryId: number;
    itemName: string;
    itemValue: string;
    description?: string;
}

export interface AttributeCategory {
    attributeCategoryId: number;
    categoryName: string;
    description?: string;
}

export interface AttributeCategoryParam {
    categoryName: string;
}