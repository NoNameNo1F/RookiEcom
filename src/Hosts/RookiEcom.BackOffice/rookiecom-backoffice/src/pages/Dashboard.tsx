import { Box, Drawer, List, ListItem, ListItemIcon, ListItemText, Toolbar, Typography } from "@mui/material";
import DashboardIcon from '@mui/icons-material/Dashboard';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import CategoryIcon from '@mui/icons-material/Category';
import PeopleIcon from '@mui/icons-material/People';
import { useAuth } from "react-oidc-context";
import withAuth from "../oidc/withAuth";
import { IJwtPayloadModel } from "../interfaces";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";
import { Header, Sidebar } from "../components";

const DashboardPage = () => {
    const auth = useAuth();
    const navigate = useNavigate();

    const decodedToken = auth.user?.access_token ? jwtDecode<IJwtPayloadModel>(auth.user.access_token) : null;
    const role = decodedToken?.role;

    const navItems = [
        { text: "Dashboard", path: "/", icon: <DashboardIcon /> },
        { text: "Orders", path: "/orders", icon: <ShoppingCartIcon /> },
        { text: "Products", path: "/products", icon: <CategoryIcon /> },
        { text: "Categories", path: "/categories", icon: <CategoryIcon /> },
        ...(role === "Admin" ? [{ text: "Users", path: "/users", icon: <PeopleIcon /> }] : []),
    ];
    
    return (
    <Box sx={{ display: 'flex', minHeight: '100vh' }}>
            <Header />
            <Sidebar />
            <Box component="main" sx={{ flexGrow: 1, p: 3, backgroundColor: '#f8f9fa' }}>
                <Toolbar />
                <Box sx={{ maxWidth: 1200, mx: 'auto' }}>
                    <Typography variant="h4" gutterBottom>
                        Dashboard
                    </Typography>
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2, mb: 4 }}>
                        <Box sx={{ flex: '1 1 300px', p: 2, backgroundColor: '#fff', borderRadius: 1, boxShadow: 1 }}>
                            <Typography variant="h6">Revenues</Typography>
                            <Typography variant="body1">Placeholder for revenue data</Typography>
                        </Box>
                        <Box sx={{ flex: '1 1 300px', p: 2, backgroundColor: '#fff', borderRadius: 1, boxShadow: 1 }}>
                            <Typography variant="h6">Orders</Typography>
                            <Typography variant="body1">Placeholder for order data</Typography>
                        </Box>
                        {/* Customers Widget */}
                        <Box sx={{ flex: '1 1 300px', p: 2, backgroundColor: '#fff', borderRadius: 1, boxShadow: 1 }}>
                            <Typography variant="h6">Customers</Typography>
                            <Typography variant="body1">Placeholder for customer data</Typography>
                        </Box>
                    </Box>
                    {/* Products Widget */}
                    <Box sx={{ p: 2, backgroundColor: '#fff', borderRadius: 1, boxShadow: 1 }}>
                        <Typography variant="h6">Products</Typography>
                        <Typography variant="body1">Placeholder for product data</Typography>
                    </Box>
                </Box>
            </Box>
        </Box>
    );
};

export default withAuth(DashboardPage);