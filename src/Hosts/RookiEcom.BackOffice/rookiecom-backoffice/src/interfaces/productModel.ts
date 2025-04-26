import { ProductStatus } from "../enums";
import { IProductAttribute, IVariationOption } from "./index";

interface IProductModel {
    id: number;
    sku: string;
    name: string;
    description: string;
    price: number;
    marketPrice: number;
    status: ProductStatus;
    sold: number;
    stock: number;
    categoryId: number;
    isFeature: boolean;
    images: string[];
    productAttributes: IProductAttribute[];
    productOption: IVariationOption;
};

interface IProductCreateForm {
    sku: string;
    name: string;
    description: string;
    price: number;
    marketPrice: number;
    stock: number;
    categoryId: number;
    isFeature: boolean;
    imageFiles: FileList;
    productAttributes: IProductAttribute[];
    variationOption: IVariationOption;
};

interface IProductUpdateForm {
  id: number;
  sku: string;
  name: string;
  description: string;
  price: number;
  marketPrice: number;
    status: ProductStatus;
  stock: number;
  categoryId: number;
  isFeature: boolean;
  oldImages: string[];
  newImageFiles?: FileList;
  productAttributes: IProductAttribute[];
  variationOption: IVariationOption;
}

export type {
    IProductModel,
    IProductCreateForm,
    IProductUpdateForm
}