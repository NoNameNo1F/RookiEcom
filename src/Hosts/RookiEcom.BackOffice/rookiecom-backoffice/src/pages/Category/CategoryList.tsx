import { useState } from "react";
import { useGetCategories } from "../../hooks/";
import {  Alert, Box, Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, Typography } from "@mui/material";
import { ICategoryModel } from "../../interfaces";
import { useNavigate } from "react-router-dom";
import { useDeleteCategory } from "../../hooks/useCategoryService";
import withAuth from "../../oidc/withAuth";

const CategoryListPage = () => {
    const navigate = useNavigate();
    const [pageNumber, setPageNumber] = useState(1);
    const [rowsPerPage, setRowsPerPage] = useState(25);
    
    const { data: pagedResult, isLoading, error } = useGetCategories(pageNumber, rowsPerPage);
    const categories = pagedResult?.items ?? [];
    const pageData = pagedResult?.pageData ?? {
        totalCount: 0,
        pageNumber: 1,
        pageSize: 25,
        totalPages: 0,
        hasPrevious: false,
        hasNext: false
    };

    const deleteCategoryMutation = useDeleteCategory();
    const handleDelete = (categoryId: number) => {
        if (window.confirm('Are you sure you want to delete this category?')) {
            deleteCategoryMutation.mutate(categoryId);
        }
    };

    const handleChangePage = (_event: unknown, newPage: number) => {
        setPageNumber(newPage + 1);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPageNumber(1);
    };

    if (error)
    {
         return <Alert severity="error">{error.message}</Alert>;
    }
    
    return (
        <Box sx={{ maxWidth: 1200, mx: 'auto', py: 2 }}>
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
                            <TableCell>Image</TableCell>
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
                        ) : categories.length === 0 ? (
                            <TableRow>
                                <TableCell colSpan={5}>No categories found</TableCell>
                            </TableRow>
                        ) : (
                            categories.map((category: ICategoryModel) => (
                                <TableRow key={category.id}>
                                    <TableCell>{category.id}</TableCell>
                                    <TableCell>
                                        {(category.image) ? (
                                            <img
                                            srcSet={`${category.image}?w=164&h=164&fit=crop&auto=format&dpr=2 2x`}
                                            src={`${category.image}?w=164&h=164&fit=crop&auto=format`}
                                            alt={category.name}
                                            loading="lazy"
                                            />
                                        ) : (
                                                <img
                                            srcSet={`https://th.bing.com/th/id/OIP.EL451xjWSajIv6TdPO323wHaHa?rs=1&pid=ImgDetMain?w=164&h=164&fit=crop&auto=format&dpr=2 2x`}
                                            src={`https://th.bing.com/th/id/OIP.EL451xjWSajIv6TdPO323wHaHa?rs=1&pid=ImgDetMain?w=164&h=164&fit=crop&auto=format`}
                                            alt={category.name}
                                            loading="lazy"
                                            />
                                        )}
                                    </TableCell>
                                    <TableCell>{category.name}</TableCell>
                                    <TableCell>{category.description || "N/A"}</TableCell>
                                    <TableCell>
                                        {categories.find((c) => c.id === category.parentId)?.name || "None"}
                                    </TableCell>
                                    <TableCell>{category.isPrimary ? "Yes" : "No"}</TableCell>
                                    <TableCell>
                                        <Button onClick={() => navigate(`/categories/edit/${category.id}`)}>
                                            Edit
                                        </Button>
                                        <Button color="error" onClick={() => handleDelete(category.id)}>
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
    );
};

export default withAuth(CategoryListPage);