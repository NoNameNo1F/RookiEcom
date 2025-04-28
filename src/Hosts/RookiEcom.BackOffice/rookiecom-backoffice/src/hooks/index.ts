import { useGetCategories, useGetCategoryTree, useCreateCategory, useGetCategoryById, useUpdateCategory, useDeleteCategory } from "./useCategoryService";
import { useCreateProduct, useGetProductsPaging, useGetProductBySKU, useGetProductsByCategory, useUpdateProduct, useDeleteProduct } from "./useProductService";
import { useGetCustomers, useGetProfile } from "./useUserService";

export {
    useGetCategories,
    useGetCategoryTree,
    useGetCategoryById,
    useCreateCategory,
    useUpdateCategory,
    useDeleteCategory,
    useGetProductsPaging,
    useGetProductsByCategory,
    useGetProductBySKU,
    useCreateProduct,
    useUpdateProduct,
    useDeleteProduct,
    useGetCustomers,
    useGetProfile
};