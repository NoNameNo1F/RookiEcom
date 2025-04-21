export default interface ICategoryModel {
    id: number;
    name: string;
    parentId?: number;
    isPrimary: boolean;
    isProductListingEnabled: boolean;
    image?: string;
    hasChild: boolean;
}