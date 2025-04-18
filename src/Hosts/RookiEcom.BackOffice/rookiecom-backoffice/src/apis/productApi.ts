import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query";
import { WEBAPI_V1 } from "../constants/baseUrls";
import { IApiResponse } from "../interfaces";

const productApi = createApi({
    reducerPath: "productApi",
    baseQuery: fetchBaseQuery({
        baseUrl: `${WEBAPI_V1}products`,
        credentials: "include",
    }),
    endpoints: (builder) => ({
        getProducts: builder.query<IApiResponse, {
            pageNumber: number, pageSize: number }> ({
            query: (userCredentials) => ({
                url: "/",
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                },
                params: userCredentials,
            })
        }),
        createProduct: builder.mutation<IApiResponse, {
            name: string, sku: string, description: string, price: number, marketPrice: number, stock: number,
            categoryId: number,
            images: string[],
            productAttributes: object[],
            variationOptions: object[] }>({
            query: (productModel) => ({
                url: "/create",
                method: "POST",
                headers: {
                    "Content-Type": "application"
                },
                body: productModel,
            })
        }),
    })
});

export const {
    useGetProductsQuery,
    useCreateProductMutation
} = productApi;

export default productApi;