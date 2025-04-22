import { useAuth } from "react-oidc-context";
import { useNavigate } from "react-router-dom";
import DashboardIcon from '@mui/icons-material/Dashboard';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import CategoryIcon from '@mui/icons-material/Category';
import PeopleIcon from '@mui/icons-material/People';
import { Box, Drawer, List, ListItem, ListItemIcon, ListItemText, Toolbar } from "@mui/material";
import { IJwtPayloadModel } from "../../interfaces";
import { jwtDecode } from "jwt-decode";

const Sidebar = () => {
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
        <Drawer
            variant="permanent"
            sx={{
                width: 240,
                flexShrink: 0,
                [`& .MuiDrawer-paper`]: { width: 240, boxSizing: 'border-box', backgroundColor: '#343a40', color: '#fff' },
            }}
        >
            <Toolbar />
            <Box sx={{ overflow: 'auto' }}>
                <List>
                    {navItems.map((item) => {
                        return (
                            <ListItem
                            component="button"
                                key={item.text} onClick={() => navigate(item.path)}>
                                <ListItemIcon sx={{ color: '#fff' }}>{item.icon}</ListItemIcon>
                                <ListItemText primary={item.text} />
                            </ListItem>
                        );
                    })}
                </List>
            </Box>
        </Drawer>
    )
};

export default Sidebar;