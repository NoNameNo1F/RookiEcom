import { apiWebClient } from "../apis/apiClient";
import { IApiResponse, ICategoryModel } from "../interfaces";
import { PagedResult } from "../interfaces/pagedResult";

export class CategoryService {
    async getCategories(pageNumber: number = 1, pageSize: number = 10): Promise<PagedResult<ICategoryModel>> {
    const response = await apiWebClient(`/api/v1/categories?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
      method: 'GET',
    }) as IApiResponse;
    return response.result as PagedResult<ICategoryModel>;
  }

  async getCategoryById(categoryId: number): Promise<ICategoryModel> {
    const response = await apiWebClient(`/api/v1/categories/${categoryId}`, {
      method: 'GET',
    }) as IApiResponse;
    return response.result as ICategoryModel;
  }

  async getCategoryTree(categoryId: number): Promise<ICategoryModel[]> {
    const response = await apiWebClient(`/api/v1/categories/${categoryId}/tree`, {
      method: 'GET',
    }) as IApiResponse;
    return response.result as ICategoryModel[];
  }

  async createCategory(category: Omit<ICategoryModel, 'id' | 'hasChild'> & { imageFile?: File }): Promise<IApiResponse> {
    const formData = new FormData();
    formData.append('Name', category.name);
    formData.append('Description', category.description || '');
    formData.append('ParentId', category.parentId.toString());
    formData.append('IsPrimary', category.isPrimary.toString());
    if (category.imageFile) formData.append('Image', category.imageFile);

    return apiWebClient('/api/v1/categories', {
      method: 'POST',
      body: formData,
    }) as Promise<IApiResponse>;
  }

  async updateCategory(categoryId: number, category: Omit<ICategoryModel, 'id' | 'hasChild'> & { imageFile?: File }): Promise<IApiResponse> {
    const formData = new FormData();
    formData.append('Name', category.name);
    formData.append('Description', category.description || '');
    formData.append('ParentId', category.parentId.toString());
    formData.append('IsPrimary', category.isPrimary.toString());
    if (category.imageFile) formData.append('Image', category.imageFile);

    return apiWebClient(`/api/v1/categories/${categoryId}`, {
      method: 'PUT',
      body: formData,
    }) as Promise<IApiResponse>;
  }

  async deleteCategory(categoryId: number): Promise<IApiResponse> {
    return apiWebClient(`/api/v1/categories/${categoryId}`, {
      method: 'DELETE',
    }) as Promise<IApiResponse>;
  }
}