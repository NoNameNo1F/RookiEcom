import { IProductAttribute, IVariationOption } from "./index";

interface IProductModel {
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
    isFeature: boolean;
    imageGallery: string[];
    productAttributes: IProductAttribute[];
    variationOption: IVariationOption;
};


interface IProductUpdateModel {
  id: string;
  sku: string;
  name: string;
  description: string;
  price: number;
  marketPrice: number;
  status: number;
  stock: number;
  categoryId: number;
  oldImages: string[];
  newImages: File[];
  productAttributes: IProductAttribute[];
  variationOption: IVariationOption;
  isFeature: boolean;
}

export type {
    IProductModel,
    IProductUpdateModel
}