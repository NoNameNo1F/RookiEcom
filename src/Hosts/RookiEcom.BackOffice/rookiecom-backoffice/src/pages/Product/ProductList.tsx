import { useState } from "react";
import { useGetProductsPaging } from "../../hooks/useProductService";
import { MiniLoaderPage } from "../../components/common";
import { Alert, Box, Button, Pagination, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
import { IProductModel } from "../../interfaces";
import { Link } from "react-router-dom";
import withAuth from "../../oidc/withAuth";

const ProductListPage = () => {
    const [pageNumber, setPageNumber] = useState(1);
    const pageSize = 25;

    const { data, isLoading, error } = useGetProductsPaging(pageNumber, pageSize);

    // const deleteProduct = (productId: number) => {
    //     if (window.confirm('Are you sure you want to delete this product?')) {
    //         deleteProductMutation.mutate(productId);
    //     }
    // }

    if (isLoading) {
        return <MiniLoaderPage text="Loading..." />;
    }

    if (error) {
        return <Alert severity="error">{error.message}</Alert>;
    }

    const products = data?.result!.items as Array<IProductModel> || [];
    const totalPages = Math.ceil((data?.result!.pageData!.count as number || 0) / pageSize);

    return (
        <Box sx={{ maxWidth: 1200, mx: 'auto', mt: 4, p: 2 }}>
            <Typography variant="h4" gutterBottom>
                Product List
            </Typography>
            <Button variant="contained" color="primary" component={Link} to="/products/create" sx={{ mb: 2 }}>
                Create Product
            </Button>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Name</TableCell>
                        <TableCell>SKU</TableCell>
                        <TableCell>Price</TableCell>
                        <TableCell>Stock</TableCell>
                        <TableCell>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {products.map((product: IProductModel) => {
                        return (
                            <TableRow key={product.id}>
                                <TableCell>{product.name}</TableCell>
                                <TableCell>{product.sku}</TableCell>
                                <TableCell>{product.price}</TableCell>
                                <TableCell>{product.stock}</TableCell>
                                <TableCell>
                                    <Button component={Link} to={`/products/edit/${product.id}`} color="primary">
                                        Edit
                                    </Button>
                                    {/* <Button onClick={() => handleDelete(product.id)} color="error">
                                        Delete
                                    </Button> */}
                                </TableCell>
                            </TableRow>
                        );
                    })}
                </TableBody>
            </Table>
            <Pagination
                count={totalPages}
                page={pageNumber}
                onChange={(event, value) => setPageNumber(value)}
                sx={{ mt: 2 }}
            />
        </Box>
    );
};

export default withAuth(ProductListPage);