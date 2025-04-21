import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { apiWebClient } from "../apis/apiClient";
import { IApiResponse, IProductModel } from "../interfaces";

export const useGetProductsPaging = (pageNumber: number, pageSize: number) => {
    return useQuery({
        queryKey: ['products', pageNumber, pageSize],
        queryFn: () =>
            apiWebClient('/api/v1/products?pageNumber=${pageNumber}&pageSize=${pageSize}', {
                method: 'GET',
            }) as Promise<IApiResponse>
    });
};

export const useCreateProduct = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (product: Omit<IProductModel, 'id'>) =>
            apiWebClient('api/v1/products', {
                method: 'POST',
                body: JSON.stringify(product),
            }) as Promise<IProductModel>,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['products'] });
        },
    });
};

