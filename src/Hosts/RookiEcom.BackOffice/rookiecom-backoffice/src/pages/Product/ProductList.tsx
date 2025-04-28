import { useState } from "react";
import { Box, Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, FormControl, InputLabel, MenuItem, Paper, Select, TextField, Typography } from "@mui/material";
import { ICategoryModel } from "../../interfaces";
import { useNavigate } from "react-router-dom";
import withAuth from "../../oidc/withAuth";
import { useDeleteProduct, useGetCategories } from "../../hooks";
import { ProductTable } from "../../components/products";
import AddIcon from '@mui/icons-material/Add';

const ProductListPage = () => {
    const navigate = useNavigate();
    const [categoryId, setCategoryId] = useState<number>(0);
    const [searchTerm, setSearchTerm] = useState<string>("");

    const [deleteConfirmOpen, setDeleteConfirmOpen] = useState(false);
    const [productToDelete, setProductToDelete] = useState<{ id: number; name: string } | null>(null);

    const { data: categoriesResult } = useGetCategories(1, 100);
    const categories = categoriesResult?.items ?? [];

    const deleteProductMutation = useDeleteProduct();

    const handleOpenDeleteConfirm = (productId: number, productName: string) => {
        setProductToDelete({ id: productId, name: productName });
        setDeleteConfirmOpen(true);
    };

    const handleCloseDeleteConfirm = () => {
        setProductToDelete(null);
        setDeleteConfirmOpen(false);
    };

    const handleConfirmDelete = () => {
        if (productToDelete) {
            deleteProductMutation.mutate(productToDelete.id, {
                onSuccess: () => {
                    handleCloseDeleteConfirm();
                },
                onError: () => {
                    handleCloseDeleteConfirm()
                }
            });
        }
    };

    return (
        <>
        <Paper sx={{ p: { xs: 2, sm: 3 }, margin: { xs: 1, sm: 3 } }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3, flexWrap: 'wrap', gap: 2 }} >
                    <Typography variant="h4" component="h1" gutterBottom>Products</Typography>
                    <Button variant="contained" startIcon={<AddIcon />} onClick={() => navigate('/products/create')} > Create Product </Button>
                </Box>
                <Box sx={{ mb: 3, display: 'flex', gap: 2, flexWrap: 'wrap', alignItems: 'center' }}>
                    <FormControl sx={{ minWidth: 200 }} size='small'>
                         <InputLabel>Filter by Category</InputLabel>
                         <Select value={categoryId} onChange={(e) => setCategoryId(e.target.value as number | 0)} label="Filter by Category">
                             <MenuItem value={0}>All Categories</MenuItem>
                             {categories.map((cat: ICategoryModel) => ( <MenuItem key={cat.id} value={cat.id}>{cat.name}</MenuItem> ))}
                         </Select>
                     </FormControl>
                     <TextField label="Search by SKU or Name" variant="outlined" size="small" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} sx={{ flexGrow: 1, minWidth: 240 }} />
                </Box>

            <ProductTable
                categoryId={categoryId}
                searchTerm={searchTerm}
                onDeleteProduct={handleOpenDeleteConfirm} 
                isDeleting={deleteProductMutation.isPending}
            />
        </Paper>

        <Dialog
            open={deleteConfirmOpen}
            onClose={handleCloseDeleteConfirm}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
        >
            <DialogTitle id="alert-dialog-title">
                Confirm Deletion
            </DialogTitle>
            <DialogContent>
                <DialogContentText id="alert-dialog-description">
                    Are you sure you want to delete the product "{productToDelete?.name}" (ID: {productToDelete?.id})? This action cannot be undone.
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleCloseDeleteConfirm} disabled={deleteProductMutation.isPending}>Cancel</Button>
                <Button onClick={handleConfirmDelete} color="error" autoFocus disabled={deleteProductMutation.isPending}>
                    {deleteProductMutation.isPending ? <CircularProgress size={20} color="inherit" /> : 'Delete'}
                </Button>
            </DialogActions>
        </Dialog>
        </>
    );
};

export default withAuth(ProductListPage);