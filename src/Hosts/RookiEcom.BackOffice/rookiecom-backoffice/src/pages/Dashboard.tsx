import { Box, Grid, Paper, Typography } from "@mui/material";
import withAuth from "../oidc/withAuth";
import { ProductTable } from "../components/products";

const DashboardPage: React.FC = () => {
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
                <Grid size={{xs: 12, sm:6, md: 4}}>
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
                <Grid size={{xs: 12, sm:6, md: 4}}>
                    <Paper sx={{ p: 2, borderRadius: 2, boxShadow: 1, height: '100%' }}>
                        <Typography variant="h6" component="h2" gutterBottom>
                            Orders
                        </Typography>
                        <Typography variant="body1" color="text.secondary">
                            Update Later
                        </Typography>
                    </Paper>
                </Grid>
                <Grid size={{xs: 12, sm:6, md: 4}}>
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
                <ProductTable categoryId={0} searchTerm={""} onDeleteProduct={function (productId: number, productName: string): void {
                    throw new Error("Function not implemented.");
                } } />
            </Paper>
        </Box>
    );
};

export default withAuth(DashboardPage);