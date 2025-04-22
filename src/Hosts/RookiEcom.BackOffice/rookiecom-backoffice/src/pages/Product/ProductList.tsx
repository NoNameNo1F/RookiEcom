import { useState } from "react";
import { useGetProductsPaging } from "../../hooks/useProductService";
import { Alert, Box, Button, FormControl, InputLabel, MenuItem, Paper, Select, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, TextField, Typography } from "@mui/material";
import { ICategoryModel, IProductModel } from "../../interfaces";
import { useNavigate } from "react-router-dom";
import withAuth from "../../oidc/withAuth";
import { useGetCategories } from "../../hooks";

const ProductListPage = () => {
    const navigate = useNavigate();
    const [pageNumber, setPageNumber] = useState(1);
    const [rowsPerPage, setRowsPerPage] = useState(25);
    const [categoryId, setCategoryId] = useState<number | "">("");

    const { data: pagedResult , isLoading: productsLoading, error } = useGetProductsPaging(pageNumber, rowsPerPage);
    const { data: categoriesResult} = useGetCategories(1, 100);

    const products = pagedResult?.items ?? [];
    const pageData = pagedResult?.pageData ?? { totalCount: 0, pageNumber: 1, pageSize: 25, totalPages: 0, hasPrevious: false, hasNext: false };
    const categories = categoriesResult?.items ?? [];

    if (error) {
        return <Alert severity="error">{error.message}</Alert>;
    }

    const handleChangePage = (_event: unknown, newPage: number) => {
        setPageNumber(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 30));
        setPageNumber(1);
    };

    return (
        <Box sx={{ display: 'flex', minHeight: '100vh' }}>
            <Box component="main" sx={{ flexGrow: 1, p: 3, backgroundColor: '#f8f9fa' }}>

            <Typography variant="h4" gutterBottom>
                Products
            </Typography>
            <Box sx={{ mb: 2, display: 'flex', gap: 2 }}>
                <Button variant="contained" onClick={() => navigate("/products/create")}>
                    Create Product
                </Button>
                <FormControl sx={{ minWidth: 200 }}>
                    <InputLabel>Category</InputLabel>
                    <Select
                        value={categoryId}
                        onChange={(e) => setCategoryId(e.target.value as number | "")}
                        label="Category"
                    >
                        <MenuItem value="">All Categories</MenuItem>
                        {categories.map((cat: ICategoryModel) => (
                            <MenuItem key={cat.id} value={cat.id}>{cat.name}</MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <TextField label="Search by SKU or Name" variant="outlined" />
            </Box>
            <TableContainer component={Paper}>

            <Table>
                <TableHead>
                    <TableRow>
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
                            {productsLoading ? (
                                <TableRow>
                                    <TableCell colSpan={6}>Loading...</TableCell>
                                </TableRow>
                            ) : products.length === 0 ? (
                                <TableRow>
                                    <TableCell colSpan={6}>No products found</TableCell>
                                </TableRow>
                            ) : (
                    products.map((product: IProductModel) => (
                            <TableRow key={product.id}>
                                <TableCell>{product.name}</TableCell>
                                <TableCell>{product.sku}</TableCell>
                                <TableCell>{product.price}</TableCell>
                                <TableCell>{product.stock}</TableCell>
                                <TableCell>{product.isFeature}</TableCell>
                                <TableCell>
                                    <Button onClick={() => navigate(`/products/edit/${product.id}`)} color="primary">
                                        Edit
                                    </Button>
                                    {/* <Button onClick={() => handleDelete(product.id)} color="error">
                                        Delete
                                    </Button> */}
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
                    page={pageNumber}
                    onPageChange={handleChangePage}
                    onRowsPerPageChange={handleChangeRowsPerPage}
                />
            </Box>
            </Box>
    );
};

export default withAuth(ProductListPage);