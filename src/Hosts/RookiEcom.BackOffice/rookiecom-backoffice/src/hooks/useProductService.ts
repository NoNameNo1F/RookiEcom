import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { IProductCreateForm, IProductUpdateForm } from "../interfaces/productModel";
import { ProductService } from "../services";
import { apiWebClient, ProblemDetailsError } from "../apis/apiClient";
import { toast } from "react-toastify";

const productService = new ProductService(apiWebClient);
export const useGetProductsPaging = (pageNumber: number = 1, pageSize: number = 25, ) => {
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
        enabled: categoryId > 0,
    });
};

export const useGetProductBySKU = (sku: string) => {
    return useQuery({
        queryKey: ['product', sku],
        queryFn: () =>
            productService.getProductBySKU(sku),
        enabled: !!sku,
    });
};

export const useCreateProduct = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (product: IProductCreateForm) =>
            productService.createProduct(product),
        onSuccess: () => {
            toast.success('Product created successfully!');
            queryClient.invalidateQueries({ queryKey: ['products'] });
        },
        onError: (error) => {
            if (error instanceof ProblemDetailsError) {
                 toast.error(`Error: ${error.problemDetails.title || error.message}`);
             } else {
                 toast.error(`Failed to create product: ${(error as Error).message}`);
             }
        }
    });
};


export const useUpdateProduct = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (product: IProductUpdateForm) =>
            productService.updateProduct(product.id, product),
        onSuccess: (data, variables) => {
            toast.success(`Product "${variables.name}" updated successfully!`);
            queryClient.invalidateQueries({ queryKey: ['products'] });
            queryClient.invalidateQueries({ queryKey: ['product', variables.sku] });
        },
         onError: (error, variables) => {
            const productName = variables?.name || `ID ${variables?.id}`;
            if (error instanceof ProblemDetailsError) {
                 toast.error(`Error updating ${productName}: ${error.problemDetails.title || error.message}`);
             } else {
                toast.error(`Failed to update ${productName}: ${(error as Error).message}`);
             }
        },
    });
};

export const useDeleteProduct = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (productId: number) =>
            productService.deleteProduct(productId),
        onSuccess: (data, productId) => {
            toast.success(`Product ID: ${productId} deleted successfully!`);
            queryClient.invalidateQueries({ queryKey: ['products'] });
            queryClient.invalidateQueries({ queryKey: ['product', productId] });
        },
        onError: (error, productId) => {
            if (error instanceof ProblemDetailsError) {
                 toast.error(`Error deleting product ${productId}: ${error.problemDetails.title || error.message}`);
             } else {
                toast.error(`Failed to delete product ${productId}: ${(error as Error).message}`);
            }
        },
    });
};