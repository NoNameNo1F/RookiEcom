import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { ICategoryCreateForm, ICategoryUpdateForm } from '../interfaces';
import { CategoryService } from '../services';
import { apiWebClient, ProblemDetailsError } from '../apis/apiClient';
import { toast } from 'react-toastify';

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
        enabled: !!categoryId
    });
};

export const useGetCategoryById = (categoryId: number) => {
    return useQuery({
        queryKey: ['category', categoryId],
        queryFn: () => categoryService.getCategoryById(categoryId),
        enabled: !!categoryId
    });
};

export const useCreateCategory = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (category: ICategoryCreateForm) => categoryService.createCategory(category),
        onSuccess: () => {
            toast.success('Category created successfully!');
            queryClient.invalidateQueries({ queryKey: ['categories'] });
        },
        onError: (error) => {
            if (error instanceof ProblemDetailsError) {
                 toast.error(`Error: ${error.problemDetails.title || error.message}`);
             } else {
                 toast.error(`Failed to create category: ${(error as Error).message}`);
             }
        }
    });
};

export const useUpdateCategory = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (category: ICategoryUpdateForm) => 
            categoryService.updateCategory(category.id, category),
        onSuccess: (data, variables) => {
            toast.success(`Category "${variables.name}" updated successfully!`);
            queryClient.invalidateQueries({ queryKey: ['categories'] });
            queryClient.invalidateQueries({ queryKey: ['category', variables.id] });
        },
        onError: (error, variables) => {
            const categoryName = variables?.name || `ID ${variables?.id}`;
            if (error instanceof ProblemDetailsError) {
                 toast.error(`Error updating ${categoryName}: ${error.problemDetails.title || error.message}`);
             } else {
                toast.error(`Failed to update ${categoryName}: ${(error as Error).message}`);
             }
        },
    });
};

export const useDeleteCategory = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (categoryId: number) => categoryService.deleteCategory(categoryId),
        onSuccess: (data, categoryId) => {
            toast.success(`Category ID: ${categoryId} deleted successfully!`);
            queryClient.invalidateQueries({ queryKey: ['categories'] });
            queryClient.invalidateQueries({ queryKey: ['category', categoryId] });
        },
        onError: (error, categoryId) => {
            if (error instanceof ProblemDetailsError) {
                 toast.error(`Error deleting category ${categoryId}: ${error.problemDetails.title || error.message}`);
             } else {
                toast.error(`Failed to delete category ${categoryId}: ${(error as Error).message}`);
            }
        },
    });
};