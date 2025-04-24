import { Box, Grid, Paper, Typography } from "@mui/material";

import withAuth from "../oidc/withAuth";

import { ProductTable } from "../components/products";

const DashboardPage: React.FC = () => {
    return (
        <Box sx={{ maxWidth: 1200, m: 0, py: 2 }}>
            <Typography variant="h4" gutterBottom>
                Dashboard
            </Typography>
            <Grid container spacing={2} sx={{ mb: 4 }}>
                <Grid size={{xs: 12, sm:6, md: 4}}>
                <Paper sx={{ p: 2, borderRadius: 2, boxShadow: 1 }}>
                    <Typography variant="h6">Revenues</Typography>
                    <Typography variant="body1">Placeholder for revenue data</Typography>
                </Paper>
                </Grid>
                <Grid size={{xs: 12, sm:6, md: 4}}>
                    <Paper sx={{ p: 2, borderRadius: 2, boxShadow: 1 }}>
                        <Typography variant="h6">Orders</Typography>
                        <Typography variant="body1">Placeholder for order data</Typography>
                    </Paper>
                </Grid>
                <Grid size={{xs: 12, sm:6, md: 4}}>
                <Paper sx={{ p: 2, borderRadius: 2, boxShadow: 1 }}>
                    <Typography variant="h6">Customers</Typography>
                    <Typography variant="body1">Placeholder for customer data</Typography>
                </Paper>
                </Grid>
            </Grid>
            <Paper sx={{ p: 2, borderRadius: 2, boxShadow: 1 }}>
                <Typography variant="h6" gutterBottom>
                Products
                </Typography>
                <ProductTable />
            </Paper>
        </Box>
    );
};

export default withAuth(DashboardPage);