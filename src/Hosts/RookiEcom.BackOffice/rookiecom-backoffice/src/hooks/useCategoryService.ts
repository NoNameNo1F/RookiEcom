import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { apiWebClient } from '../apis/apiClient';
import { IApiResponse, ICategoryModel } from '../interfaces';

export const useGetCategories = () => {
  return useQuery({
    queryKey: ['categories'],
    queryFn: () =>
      apiWebClient('/api/v1/categories', {
        method: 'GET',
      }) as Promise<IApiResponse>,
  });
};

export const useGetCategoryTree = (categoryId: number) => {
  return useQuery({
    queryKey: ['categoryTree', categoryId],
    queryFn: () =>
      apiWebClient(`/api/v1/categories/${categoryId}/tree`, {
        method: 'GET',
      }) as Promise<ICategoryModel[]>,
    enabled: !!categoryId,
  });
};

export const useCreateCategory = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (category: Omit<ICategoryModel, 'id'>) =>
      apiWebClient('/api/v1/categories', {
        method: 'POST',
        body: JSON.stringify(category),
      }) as Promise<ICategoryModel>,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['categories'] });
    },
  });
};