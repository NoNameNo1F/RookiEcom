import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { ICategoryModel } from '../interfaces';

import { CategoryService } from '../services';
import { apiWebClient } from '../apis/apiClient';

const categoryService = new CategoryService(apiWebClient);



export const useGetCategories = (pageNumber: number = 1, pageSize:number = 25) => {
  return useQuery({
    queryKey: ['categories', pageNumber, pageSize],
    queryFn: () => categoryService.getCategories(pageNumber, pageSize),
  });
};

export const useGetCategoryTree = (categoryId: number) => {
  return useQuery({
    queryKey: ['categoryTree', categoryId],
    queryFn: () => categoryService.getCategoryTree(categoryId),
  });
};

export const useGetCategoryById = (categoryId: number) => {
    return useQuery({
        queryKey: ['category', categoryId],
        queryFn: () => categoryService.getCategoryById(categoryId),
    });
};

export const useCreateCategory = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (category: Omit<ICategoryModel, 'id' | 'hasChild'> & { imageFile?: File }) => categoryService.createCategory(category),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['categories'] });
        },
    });
};

export const useUpdateCategory = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ categoryId, category }: { categoryId: number; category: Omit<ICategoryModel, 'id' | 'hasChild'> & { imageFile?: File; }; }) => 
            categoryService.updateCategory(categoryId, category),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['categories'] });
        },
    });
};

export const useDeleteCategory = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (categoryId: number) => categoryService.deleteCategory(categoryId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['categories'] });
        },
    });
};