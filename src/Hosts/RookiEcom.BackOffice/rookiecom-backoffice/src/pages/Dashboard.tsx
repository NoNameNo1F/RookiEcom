import { Box, Grid, Paper, Typography } from "@mui/material";
import withAuth from "../oidc/withAuth";
import { ProductTable } from "../components/products";
import { useDeleteProduct } from "../hooks";
import { useState } from "react";
import { ConfirmationDialog } from "../components/common";

const DashboardPage: React.FC = () => {
    const [deleteConfirmOpen, setDeleteConfirmOpen] = useState(false);
    const [productToDelete, setProductToDelete] = useState<{ id: number; name: string } | null>(null);
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
                onSuccess: handleCloseDeleteConfirm,
                onError: handleCloseDeleteConfirm
            });
        }
    };
    return (
        <Box sx={{ p: { xs: 1, sm: 2 } }}>
            <Typography
                variant="h4"
                component="h1"
                gutterBottom sx={{ mb: 3 }}
            >
                Dashboard
            </Typography>
            <Grid container spacing={3} sx={{ mb: 4 }}>
                <Grid size={{xs: 12, sm:12, md: 4}}>
                    <Paper sx={{ p: 2.5, borderRadius: 2, boxShadow: 1, height: '100%' }}
                    >
                        <Typography variant="h6" component="h2" gutterBottom>
                            Revenues
                        </Typography>
                        <Typography variant="body1" color="text.secondary">
                            Nothing Chart
                        </Typography>
                    </Paper>
                </Grid>
                <Grid size={{xs: 12, sm:12, md: 4}}>
                    <Paper sx={{ p: 2, borderRadius: 2, boxShadow: 1, height: '100%' }}>
                        <Typography variant="h6" component="h2" gutterBottom>
                            Orders
                        </Typography>
                        <Typography variant="body1" color="text.secondary">
                            Update Later
                        </Typography>
                    </Paper>
                </Grid>
                <Grid size={{xs: 12, sm:12, md: 4}}>
                    <Paper sx={{ p: 2.5, borderRadius: 2, boxShadow: 1, height: '100%' }}>
                        <Typography variant="h6" component="h2" gutterBottom>
                            Customers
                        </Typography>
                        <Typography variant="body1" color="text.secondary">
                            No Customer Data
                        </Typography>
                    </Paper>
                </Grid>
            </Grid>
            <Paper sx={{ p: { xs: 2, sm: 3 }, borderRadius: 2, boxShadow: 1 }}>
                <Typography variant="h5" component="h2" gutterBottom>
                    Products
                </Typography>
                <ProductTable categoryId={0} searchTerm={""} onDeleteProduct={handleOpenDeleteConfirm} />
            </Paper>
            <ConfirmationDialog
                open={deleteConfirmOpen}
                onClose={handleCloseDeleteConfirm}
                onConfirm={handleConfirmDelete}
                title="Confirm Deletion"
                message="Are you sure you want to delete the product {name} (ID: {id})?"
                isLoading={deleteProductMutation.isPending}
                entityName={productToDelete?.name}
                entityId={productToDelete?.id}
            />
        </Box>
    );
};

export default withAuth(DashboardPage);