import ApiWebClient, { apiWebClient } from "../apis/apiClient";
import { IApiResponse, IProductAttribute, IProductModel } from "../interfaces";
import { PagedResult } from "../interfaces/pagedResult";
import { IProductUpdateModel } from "../interfaces/productModel";

export class ProductService {
    private readonly client: ApiWebClient;

    public constructor(client: ApiWebClient) {
        this.client = client;
    }

    public async getProductsPaging(pageNumber: number = 1, pageSize: number = 25): Promise<PagedResult<IProductModel>> {
        const response = await this.client.get(`/api/v1/products?pageNumber=${pageNumber}&pageSize=${pageSize}`) as IApiResponse;

        return response.result as PagedResult<IProductModel>;
    }

  public async getProductsByCategory(categoryId: number, pageNumber: number = 1, pageSize: number = 25): Promise<PagedResult<IProductModel>> {
      const response = await this.client.get(`/api/v1/products/c${categoryId}?pageNumber=${pageNumber}&pageSize=${pageSize}`) as IApiResponse;
      
        return response.result as PagedResult<IProductModel>;
    }

    public async getProductBySKU(sku: string): Promise<IProductModel> {
        const response = await this.client.get(`/api/v1/products/${sku}`) as IApiResponse;

        return response.result as IProductModel;
    }

    public async createProduct(product: Omit<IProductModel, 'id' | 'sold'> & { imageFiles?: File[] }) : Promise<void> {
        const formData = new FormData();
        formData.append('SKU', product.sku);
        formData.append('CategoryId', product.categoryId.toString());
        formData.append('Name', product.name);
        formData.append('Description', product.description || '');
        formData.append('MarketPrice', product.marketPrice.toString());
        formData.append('Price', product.price.toString());
        formData.append('StockQuantity', product.stock.toString());
        formData.append('IsFeature', product.isFeature?.toString() || 'false');
        if(product.imageFiles) {
            product.imageFiles.forEach((file, index) => {
                formData.append(`Images[${index}]`, file);
            });
        }
        if (product.productAttributes) {
            product.productAttributes.forEach((attr: IProductAttribute, index: number) => {
                formData.append(`ProductAttributes[${index}].Code`, attr.code);
                formData.append(`ProductAttributes[${index}].Value`, attr.value);
            });
        }
        if (product.variationOption) {
            formData.append('ProductOption.Code', product.variationOption.code);
            product.variationOption.values.forEach((value: string, index: number) => {
                formData.append(`ProductOption.Values[${index}]`, value);
            });
        }

        await this.client.post(
            'api/v1/products',
            { data: formData }, {
            headers: {
                "Content-Type": "multipart/form-data"
            }
        });
    }

    public async updateProduct(product: IProductUpdateModel): Promise<void> {
        const formData = new FormData();
        formData.append('Id', product.id.toString());
        formData.append('SKU', product.sku);
        formData.append('CategoryId', product.categoryId.toString());
        formData.append('Name', product.name);
        formData.append('Description', product.description || '');
        formData.append('MarketPrice', product.marketPrice.toString());
        formData.append('Price', product.price.toString());
        formData.append('StockQuantity', product.stock.toString());
        formData.append('IsFeature', product.isFeature.toString());
        if (product.oldImages) {
            product.oldImages.forEach((url, index) => {
                formData.append(`OldImages[${index}]`, url);
            });
        }
        
        if (product.newImages) {
            product.newImages.forEach((file, index) => {
                formData.append(`NewImages[${index}]`, file);
            });
        }
        
        if (product.productAttributes) {
            product.productAttributes.forEach((attr, index) => {
                formData.append(`ProductAttributes[${index}].Code`, attr.code);
                formData.append(`ProductAttributes[${index}].Value`, attr.value);
            });
        }
        
        if (product.variationOption) {
            formData.append('ProductOption.Code', product.variationOption.code);
            product.variationOption.values.forEach((value, index) => {
                formData.append(`ProductOption.Values[${index}]`, value);
            });
        }

        await this.client.put(
            `/api/v1/products/${product.id}`,
            { data: formData }, {
            headers: {
                "Content-Type": "multipart/form-data"
            }
        });
    }

    public async deleteProduct(productId: number): Promise<void> {
        await this.client.delete(`/api/v1/products/${productId}`)
    }
}