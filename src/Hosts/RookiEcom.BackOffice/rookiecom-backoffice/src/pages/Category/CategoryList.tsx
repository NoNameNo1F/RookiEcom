import { useState } from "react";
import { useGetCategories, useGetCategoryTree } from "../../hooks/";
import { MiniLoaderPage } from "../../components/common";
import { Alert, Box, Button, CircularProgress, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
import { ICategoryModel } from "../../interfaces";
import { Link } from "react-router-dom";
import withAuth from "../../oidc/withAuth";

const CategoryListPage = () => {
    const { data: categories, isLoading, error } = useGetCategories();
    const [selectedCategoryId, setSelectedCategoryId] = useState<number | null>(null);
    const { data: categoryTree, isLoading: treeLoading } = useGetCategoryTree(selectedCategoryId!);
    // const deleteCategoryMutation = useDeleteCategory();

    // const handleDelete = (categoryId: number) => {
    //     if (window.confirm('Are you sure you want to delete this category?')) {
    //         deleteCategoryMutation.mutate(categoryId);
    //     }
    // };

    const handleViewTree = (categoryId: number) => {
        setSelectedCategoryId(categoryId);
    };

    if (isLoading) {
        return <MiniLoaderPage text="Loading..." />;
    }

    if (error) {
        return <Alert severity="error">{error.message}</Alert>;
    }

    return (
        <Box sx={{ maxWidth: 1200, mx: 'auto', mt: 4, p: 2 }}>
            <Typography variant="h4" gutterBottom>
                Category List
            </Typography>
            <Button variant="contained" color="primary" component={Link} to="/categories/create" sx={{ mb: 2 }}>
                Create Category
            </Button>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Name</TableCell>
                        <TableCell>Parent ID</TableCell>
                        <TableCell>Is Primary</TableCell>
                        <TableCell>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {(categories?.result as Array<ICategoryModel> || []).map((category: ICategoryModel) => (
                        <TableRow key={category.id}>
                            <TableCell>{category.name}</TableCell>
                            <TableCell>{category.parentId || 'None'}</TableCell>
                            <TableCell>{category.isPrimary ? 'Yes' : 'No'}</TableCell>
                            <TableCell>
                                <Button component={Link} to={`/categories/edit/${category.id}`} color="primary">
                                    Edit
                                </Button>
                                {/* <Button onClick={() => handleDelete(category.id)} color="error">
                                    Delete
                                </Button> */}
                                <Button onClick={() => handleViewTree(category.id)} color="info">
                                    View Tree
                                </Button>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
            {selectedCategoryId && (
                <Box sx={{ mt: 4 }}>
                    <Typography variant="h5" gutterBottom>
                        Category Tree for {categories?.result.find((c: ICategoryModel) => c.id === selectedCategoryId)?.name}
                    </Typography>
                    {treeLoading ? (
                        <CircularProgress />
                    ) : (
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell>Name</TableCell>
                                    <TableCell>Parent ID</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {(categoryTree || []).map((category: ICategoryModel) => (
                                    <TableRow key={category.id}>
                                        <TableCell>{category.name}</TableCell>
                                        <TableCell>{category.parentId || 'None'}</TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    )}
                </Box>
            )}
        </Box>
    );
};

export default withAuth(CategoryListPage);