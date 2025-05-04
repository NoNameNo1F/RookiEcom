import { useState } from "react";
import { useGetCategories } from "../../hooks/";
import { Alert, Avatar, Box, Button, IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, Tooltip, Typography, useTheme } from "@mui/material";
import AddIcon from '@mui/icons-material/Add';
import ImageIcon from '@mui/icons-material/Image';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { ICategoryModel } from "../../interfaces";
import { useNavigate } from "react-router-dom";
import { useDeleteCategory } from "../../hooks/useCategoryService";
import withAuth from "../../oidc/withAuth";
import { ConfirmationDialog, MiniLoaderPage } from "../../components/common";

const CategoryListPage = () => {
    const navigate = useNavigate();
    const theme = useTheme();
    const [pageNumber, setPageNumber] = useState(1);
    const [rowsPerPage, setRowsPerPage] = useState(10);
    const [deleteConfirmOpen, setDeleteConfirmOpen] = useState(false);
    const [categoryToDelete, setCategoryToDelete] = useState<{ id: number; name: string; } | null>(null);
    
    const { data: pagedResult, isLoading, error } = useGetCategories(pageNumber, rowsPerPage);
    const categories = pagedResult?.items ?? [];
    const totalCount = pagedResult?.pageData?.totalCount ?? 0;

    const deleteCategoryMutation = useDeleteCategory();
    const handleDelete = (categoryId: number, categoryName: string) => {
        setCategoryToDelete({ id: categoryId, name: categoryName });
        setDeleteConfirmOpen(true);
    };

    const handleCloseDeleteConfirm = () => {
        setCategoryToDelete(null);
        setDeleteConfirmOpen(false);
    };

    const handleConfirmDelete = () => {
        if (categoryToDelete) {
            deleteCategoryMutation.mutate(categoryToDelete.id, {
                onSuccess: () => {
                    handleCloseDeleteConfirm();
                },
                onError: () => {
                    handleCloseDeleteConfirm();
                },
            });
        }
    };


    const handleChangePage = (_event: unknown, newPage: number) => {
        setPageNumber(newPage + 1);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPageNumber(1);
    };
    
    const columns = [
        { id: 'id', label: 'ID', width: 60, hideOnTablet: true },
        { id: 'image', label: 'Image', width: 80 },
        { id: 'name', label: 'Name', minWidth: 150 },
        { id: 'description', label: 'Description', minWidth: 200, hideOnTablet: true },
        { id: 'parent', label: 'Parent', minWidth: 100, hideOnTablet: true },
        { id: 'isPrimary', label: 'Primary', align: 'center', width: 80 },
        { id: 'actions', label: 'Actions', align: 'right', width: 120 },
    ];

    return (
        <Paper sx={{p: { xs: 2, sm: 3 },margin: { xs: 1, sm: 3 }}}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3,     flexWrap: 'wrap', gap: 2 }}
            >
                <Typography
                    variant="h4"
                    component="h1"
                    sx={{ flexGrow: { xs: 1, sm: 0 } }}
                >
                    Categories
                </Typography>
            <Button variant="contained" startIcon={<AddIcon />} onClick={() => navigate("/categories/create")}
            >
                Create Category
            </Button>
            <Box sx={{ width: '100%', overflowX: 'auto' }}>
                <TableContainer sx={{ boxShadow: 'none', border: `1px solid ${theme.palette.divider}`, minWidth: 650 }}>
                <Table stickyHeader aria-label="category table">
                    <TableHead>
                        <TableRow>
                            {columns.map((column) => (
                                <TableCell
                                    key={column.id}
                                    align={column.align as any}
                                    style={{ minWidth: column.minWidth, width: column.width, fontWeight: 'bold', whiteSpace: 'nowrap' }}
                                    sx={{
                                        ...(column.hideOnTablet && { display: { xs: 'none', md: 'table-cell' } })
                                    }}
                                >
                                    {column.label}
                                </TableCell>
                            ))}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {isLoading ? (
                            <TableRow>
                                <TableCell
                                    colSpan={columns.length}
                                    sx={{ textAlign: 'center', p: 5 }}>
                                    <MiniLoaderPage text="Loading Categories..."/>
                                </TableCell>
                            </TableRow>
                        ) : error ? (
                            <TableRow>
                                <TableCell
                                    colSpan={columns.length}
                                    sx={{ textAlign: 'center', p: 5 }}
                                >
                                    <Alert
                                        severity="error"
                                        sx={{ textAlign: 'center' }}
                                    >
                                        {(error as Error).message || "Failed to load products."}
                                    </Alert>
                                </TableCell>
                            </TableRow>
                        ) : categories.length === 0 && !error ? (
                            <TableRow>
                                <TableCell
                                    colSpan={columns.length}
                                    sx={{ textAlign: 'center', p: 5 }}
                                >
                                    No categories found
                                </TableCell>
                            </TableRow>
                        ) : (
                            categories.map((category: ICategoryModel) => (
                                <TableRow hover key={category.id}>
                                    <TableCell
                                        sx={{ display: { xs: 'none', md: 'table-cell' } }}
                                    >
                                        {category.id}
                                    </TableCell>
                                    <TableCell>
                                        {/* {(category.image) ? (
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
                                        )} */}
                                        <Avatar
                                            variant="rounded"
                                            src={category.image || "https://th.bing.com/th/id/OIP.EL451xjWSajIv6TdPO323wHaHa"}
                                            alt={category.name}
                                            sx={{ width: 56, height: 56, bgcolor: 'grey.200' }}
                                        >
                                            {!category.image && <ImageIcon color="disabled"/>}
                                        </Avatar>
                                    </TableCell>
                                    <TableCell sx={{ whiteSpace: 'nowrap' }}>{category.name}</TableCell>
                                    <TableCell
                                        sx={{ display: { xs: 'none', md: 'table-cell' } }}
                                    >
                                        {category.description || 
                                            <Typography variant="caption" color="text.secondary">
                                                N/A
                                            </Typography>
                                        }
                                    </TableCell>
                                    <TableCell
                                        sx={{ display: { xs: 'none', md: 'table-cell' } }}
                                    >
                                        {categories.find((c) => c.id === category.parentId)?.name || <Typography variant="caption" color="text.secondary">
                                            None
                                        </Typography>
                                        }
                                    </TableCell>
                                    <TableCell align="center">
                                        {category.isPrimary ? "Yes" : "No"}
                                    </TableCell>
                                    <TableCell align="right" sx={{ whiteSpace: 'nowrap' }}>
                                        <Tooltip title="Edit Category">
                                            <IconButton size="small" onClick={() => navigate(`/categories/edit/${category.id}`)} color="primary">
                                                <EditIcon fontSize="small" />
                                            </IconButton>
                                        </Tooltip>
                                        <Tooltip title="Delete Category">
                                            <IconButton size="small" onClick={() => handleDelete(category.id, category.name)} color="error" disabled={deleteCategoryMutation.isPending}>
                                                <DeleteIcon fontSize="small" />
                                            </IconButton>
                                        </Tooltip>
                                    </TableCell>
                                </TableRow>
                            ))
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
            {totalCount > 0 && (
                <TablePagination
                    rowsPerPageOptions={[5, 10, 25]}
                    component="div"
                    count={totalCount}
                    rowsPerPage={rowsPerPage}
                    page={pageNumber - 1}
                    onPageChange={handleChangePage}
                    onRowsPerPageChange={handleChangeRowsPerPage}
                    sx={{ borderTop: `1px solid ${theme.palette.divider}` }}
                />
            )}
                </Box>
            </Box>
            <ConfirmationDialog
                open={deleteConfirmOpen}
                onClose={handleCloseDeleteConfirm}
                onConfirm={handleConfirmDelete}
                title="Confirm Deletion"
                message="Are you sure you want to delete the category {name} (ID: {id})? This might affect associated products."
                isLoading={deleteCategoryMutation.isPending}
                entityName={categoryToDelete?.name}
                entityId={categoryToDelete?.id}
            />
        </Paper>
    );
};

export default withAuth(CategoryListPage);