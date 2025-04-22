import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { IProductModel } from "../interfaces";

import { IProductUpdateModel } from "../interfaces/productModel";
import { ProductService } from "../services/productService";

const productService = new ProductService();
export const useGetProductsPaging = (pageNumber: number = 1, pageSize: number = 25) => {
    return useQuery({
        queryKey: ['products', pageNumber, pageSize],
        queryFn: () =>
            productService.getProductsPaging(pageNumber, pageSize),
    });
};

export const useGetProductsByCategory = (categoryId: number, pageNumber: number = 1, pageSize: number = 25) => {
    return useQuery({
        queryKey: ['products', 'category', categoryId, pageNumber, pageSize],
        queryFn: () =>
            productService.getProductsByCategory(categoryId, pageNumber, pageSize),
    });
};

export const useGetProductBySKU = (sku: string) => {
    return useQuery({
        queryKey: ['product', sku],
        queryFn: () =>
            productService.getProductBySKU(sku),
    });
};

export const useCreateProduct = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (product: Omit<IProductModel, 'id' | 'sold'> & { imageFiles?: File[]; }) => 
            productService.createProduct(product),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['products'] });
        },
    });
};


export const useUpdateProduct = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ productId, product }: { productId: number; product: IProductUpdateModel }) => productService.updateProduct(productId, product),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['products'] });
        },
    });
};

export const useDeleteProduct = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (productId: number) =>
            productService.deleteProduct(productId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['products'] });
        },
    });
};