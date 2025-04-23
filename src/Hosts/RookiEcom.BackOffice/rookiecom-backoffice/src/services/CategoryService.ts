import ApiWebClient from "../apis/apiClient";
import { IApiResponse, ICategoryModel } from "../interfaces";
import { PagedResult } from "../interfaces/pagedResult";

export class CategoryService {
    private readonly client: ApiWebClient;

    public constructor(client: ApiWebClient) {
        this.client = client;
    }
    
    public async getCategories(pageNumber: number = 1, pageSize: number = 10): Promise<PagedResult<ICategoryModel>> {
        const response = await this.client.get(`/api/v1/categories?pageNumber=${pageNumber}&pageSize=${pageSize}`) as IApiResponse;

        return response.result as PagedResult<ICategoryModel>;
    }

    public async getCategoryById(categoryId: number): Promise<ICategoryModel> {
        const response = await this.client.get(`/api/v1/categories/${categoryId}`) as IApiResponse;
        
        return response.result as ICategoryModel;
    }

    public async getCategoryTree(categoryId: number): Promise<ICategoryModel[]> {
        const response = await this.client.get(`/api/v1/categories/${categoryId}/tree`) as IApiResponse;
        
        return response.result as ICategoryModel[];
    }

    public async createCategory(category: Omit<ICategoryModel, 'id' | 'hasChild'> & { imageFile?: File; }): Promise<void> {
        const formData = new FormData();
        formData.append('Name', category.name);
        formData.append('Description', category.description || '');
        formData.append('ParentId', category.parentId!.toString());
        formData.append('IsPrimary', category.isPrimary.toString());
        if (category.imageFile) formData.append('Image', category.imageFile);

        await this.client.post(
            '/api/v1/categories', {
            body: formData
        }, {
            headers: {
                "Content-Type": "multipart/form-data"
            }
        });
    };

    public async updateCategory(categoryId: number, category: Omit<ICategoryModel, 'id' | 'hasChild'> & { imageFile?: File }): Promise<void> {
        const formData = new FormData();
        formData.append('Name', category.name);
        formData.append('Description', category.description || '');
        formData.append('ParentId', category.parentId!.toString());
        formData.append('IsPrimary', category.isPrimary.toString());
        if (category.imageFile) formData.append('Image', category.imageFile);

        await this.client.put(
            `/api/v1/categories/${categoryId}`,
            { data: formData }, {
            headers: {
            "Content-Type": "multipart/form-data"
            }
        });
    }

    public async deleteCategory(categoryId: number): Promise<void> {
        await this.client.delete(`/api/v1/categories/${categoryId}`);
    }
}