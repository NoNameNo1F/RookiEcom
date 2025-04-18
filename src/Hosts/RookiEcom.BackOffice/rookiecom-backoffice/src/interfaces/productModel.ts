import { IProductAttribute, IVariationOption } from "./index";

export default interface IProductModel {
    id: string;
    sku: string;
    name: string;
    description: string;
    price: number;
    marketPrice: number;
    status: number;
    sold: number;
    stock: number;
    categoryId: number;
    imageGallery: string[];
    productAttributes: IProductAttribute[];
    variationOptions: IVariationOption[];
};
