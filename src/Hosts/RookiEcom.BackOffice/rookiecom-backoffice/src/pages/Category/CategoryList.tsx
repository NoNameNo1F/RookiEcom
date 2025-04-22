import { useState } from "react";
import { useGetCategories, useGetCategoryTree } from "../../hooks/";
import { MiniLoaderPage } from "../../components/common";
import { Alert, Box, Button, CircularProgress, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, Typography } from "@mui/material";
import { ICategoryModel } from "../../interfaces";
import { Link, useNavigate } from "react-router-dom";
import withAuth from "../../oidc/withAuth";

const CategoryListPage = () => {
    const navigate = useNavigate();
    const [pageNumber, setPageNumber] = useState(1);
    const [rowsPerPage, setRowsPerPage] = useState(25);
    
    const { data: categories, isLoading } = useGetCategories(pageNumber, rowsPerPage);

    // const deleteCategoryMutation = useDeleteCategory();

    // const handleDelete = (categoryId: number) => {
    //     if (window.confirm('Are you sure you want to delete this category?')) {
    //         deleteCategoryMutation.mutate(categoryId);
    //     }
    // };

    const handleChangePage = (_event: unknown, newPage: number) => {
        setPageNumber(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPageNumber(1);
    };

    return (
        <Box sx={{ display: 'flex', minHeight: '100vh' }}>
            <Box sx={{ maxWidth: 1200, mx: 'auto', mt: 4, p: 2 }}>
                <Typography variant="h4" gutterBottom>
                    Categories
                </Typography>
                <Box sx={{ mb: 2 }}>
                    <Button variant="contained" color="primary" onClick={() => navigate("/categories/create")}>
                        Create Category
                    </Button>
                </Box>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Id</TableCell>
                                <TableCell>Name</TableCell>
                                <TableCell>Description</TableCell>
                                <TableCell>Parent ID</TableCell>
                                <TableCell>Is Primary</TableCell>
                                <TableCell>Actions</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {isLoading ? (
                                <TableRow>
                                    <TableCell colSpan={5}>Loading...</TableCell>
                                </TableRow>
                            ) : categories?.items.length === 0 ? (
                                <TableRow>
                                    <TableCell colSpan={5}>No categories found</TableCell>
                                </TableRow>
                            ) : (
                                categories?.items!.map((category: ICategoryModel) => (
                                    <TableRow key={category.id}>
                                        <TableCell>{category.name}</TableCell>
                                        <TableCell>{category.description || "N/A"}</TableCell>
                                        <TableCell>
                                            {categories?.items!.find((c) => c.id === category.parentId)?.name || "None"}
                                        </TableCell>
                                        <TableCell>{category.isPrimary ? "Yes" : "No"}</TableCell>
                                        <TableCell>
                                            <Button onClick={() => navigate(`/categories/edit/${category.id}`)}>
                                                Edit
                                            </Button>
                                            <Button color="error">Delete</Button>
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
                    count={-1} // Replace with total count from API if available
                    rowsPerPage={rowsPerPage}
                    page={pageNumber}
                    onPageChange={handleChangePage}
                    onRowsPerPageChange={handleChangeRowsPerPage}
                />
            </Box>
        </Box>
    );
};

export default withAuth(CategoryListPage);