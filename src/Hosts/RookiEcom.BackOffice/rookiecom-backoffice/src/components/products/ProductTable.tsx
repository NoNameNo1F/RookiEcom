import { Alert, Box, Button, CircularProgress, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, Typography } from "@mui/material";
import { IProductModel } from "../../interfaces";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useGetProductsPaging } from "../../hooks";
import { useDeleteProduct, useGetProductsByCategory } from "../../hooks/useProductService";
import { MiniLoaderPage } from "../common";

interface ProductTableProps {
  categoryId?: number | '';
}

const ProductTable: React.FC<ProductTableProps> = ({ categoryId }) => {
    const navigate = useNavigate();
    const [pageNumber, setPageNumber] = useState(1);
    const [rowsPerPage, setRowsPerPage] = useState(25);
    
    const { data: pagedResult, isLoading, error } = categoryId
    ? useGetProductsByCategory(categoryId as number, pageNumber, rowsPerPage)
        : useGetProductsPaging(pageNumber, rowsPerPage);
    const deleteProductMutation = useDeleteProduct();

    const handleDeleteProduct = (productId: number) => {
        if (window.confirm('Are you sure you want to delete this category?')) {
            deleteProductMutation.mutate(productId);
        }
    }
    
    const products = pagedResult?.items ?? [];
    const pageData = pagedResult?.pageData ?? {
        totalCount: 0,
        pageNumber: 1,
        pageSize: 25,
        totalPages: 0,
        hasPrevious: false,
        hasNext: false
    };

    useEffect(() => {
        console.error(error);
    }, [error]);

    if (error) {
        
        return (
            <Alert severity="error">{(error as Error).message}</Alert>
        );
    }
    
     const handleChangePage = (_event: unknown, newPage: number) => {
        setPageNumber(newPage + 1);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPageNumber(1);
    };
    
    return (
        <Box>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Image</TableCell>
                            <TableCell>Name</TableCell>
                            <TableCell>SKU</TableCell>
                            <TableCell>Price</TableCell>
                            <TableCell>Stock</TableCell>
                            <TableCell>IsFeature</TableCell>
                            <TableCell>Category</TableCell>
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {isLoading ? (
                            <TableRow>
                                <TableCell colSpan={8} sx={{ textAlign: 'center' }}>
                                    <CircularProgress size={24} />
                                    <Typography variant="body2" sx={{ ml: 1, display: 'inline' }}>
                                        Loading...
                                    </Typography>
                                </TableCell>
                            </TableRow>
                        ) : !pagedResult ? (
                            <TableRow>
                                <TableCell colSpan={8}>Failed to load products</TableCell>
                            </TableRow>
                        ): products.length === 0 ? (
                            <TableRow>
                                <TableCell colSpan={8}>No products found</TableCell>
                            </TableRow>
                        ) : (
                            products.map((product: IProductModel) => (
                                <TableRow key={product.id}>
                                    <TableCell>
                                        {(product.images && product.images.length > 0) ? (
                                            <img
                                            srcSet={`${product.images[0]}?w=164&h=164&fit=crop&auto=format&dpr=2 2x`}
                                            src={`${product.images[0]}?w=164&h=164&fit=crop&auto=format`}
                                            alt={product.name}
                                            loading="lazy"
                                            />
                                        ) : (
                                                <img
                                            srcSet={`https://th.bing.com/th/id/OIP.EL451xjWSajIv6TdPO323wHaHa?rs=1&pid=ImgDetMain?w=164&h=164&fit=crop&auto=format&dpr=2 2x`}
                                            src={`https://th.bing.com/th/id/OIP.EL451xjWSajIv6TdPO323wHaHa?rs=1&pid=ImgDetMain?w=164&h=164&fit=crop&auto=format`}
                                            alt={product.name}
                                            loading="lazy"
                                            />
                                        )}
                                    </TableCell>
                                    <TableCell>{product.name}</TableCell>
                                    <TableCell>{product.sku}</TableCell>
                                    <TableCell>{product.price}</TableCell>
                                    <TableCell>{product.stock}</TableCell>
                                    <TableCell>{product.isFeature ? 'Yes' : 'No'}</TableCell>
                                     <TableCell>{product.categoryId || 'N/A'}</TableCell>
                                    <TableCell>
                                        <Button onClick={() => navigate(`/products/edit/${product.sku}`)} color="primary">
                                            Edit
                                        </Button>
                                        <Button onClick={() => handleDeleteProduct(product.id)} color="error">
                                            Delete
                                        </Button>
                                    </TableCell>
                                </TableRow>
                            ))
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
            <TablePagination
                    rowsPerPageOptions={[5, 10, 25]}
                    component="div"
                    count={pageData.totalCount}
                    rowsPerPage={rowsPerPage}
                    page={pageNumber - 1}
                    onPageChange={handleChangePage}
                    onRowsPerPageChange={handleChangeRowsPerPage}
                />
        </Box>
    )
};

export default ProductTable;