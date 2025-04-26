import { useAuth } from "react-oidc-context";
import { useLocation, useNavigate } from "react-router-dom";
import AssessmentIcon from '@mui/icons-material/Assessment';
import InventoryIcon from '@mui/icons-material/Inventory';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import CategoryIcon from '@mui/icons-material/Category';
import PeopleIcon from '@mui/icons-material/People';
import { Box, Divider, Drawer, List, ListItemButton, ListItemIcon, ListItemText, Toolbar, useMediaQuery, useTheme } from "@mui/material";
import { IJwtPayloadModel } from "../../interfaces";
import { jwtDecode } from "jwt-decode";

interface SidebarProps {
    drawerWidth: number;
    tabletOpen: boolean;
    handleDrawerToggle: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ drawerWidth, tabletOpen, handleDrawerToggle }) => {
    const auth = useAuth();
    const navigate = useNavigate();

    const location = useLocation();
    const theme = useTheme();
    
    const isMdUp = useMediaQuery(theme.breakpoints.up('md'));

    const decodedToken = auth.user?.access_token ? jwtDecode<IJwtPayloadModel>(auth.user.access_token) : null;
    const roles = decodedToken?.roles ?? [];

    const navItems = [
        { text: "Dashboard", path: "/", icon: <AssessmentIcon /> },
        { text: "Orders", path: "/orders", icon: <ShoppingCartIcon /> },
        { text: "Products", path: "/products", icon: <InventoryIcon /> },
        { text: "Categories", path: "/categories", icon: <CategoryIcon /> },
        ...(roles?.length > 0 &&roles?.includes("Admin") ? [{ text: "Users", path: "/users", icon: <PeopleIcon /> }] : []),
    ];

    const listItemStyles = {
        borderRadius: 2,
        m: 1,
        '&.MuiListItemIcon-root': {
           minWidth: '40px',
        },
        '&.Mui-selected': {
            backgroundColor: theme.palette.action.selected,
            color: theme.palette.primary.main,
            '& .MuiListItemIcon-root': {
                color: theme.palette.primary.main, 
            },
            '&:hover': {
                 backgroundColor: theme.palette.action.hover,
            }
        },
        '&:hover': {
           backgroundColor: theme.palette.action.hover,
        },
    };

    const drawerContent = (
        <div>
            <Toolbar />
            <Divider />
            <List sx={{ py: 1 }}>
                {navItems.map((item) => {
                    const isActive = location.pathname === item.path ||
                        (item.path !== "/" && location.pathname.startsWith(item.path + "/"));

                    return (
                        <ListItemButton
                            key={item.text}
                            selected={isActive}
                            onClick={() => {
                                navigate(item.path);

                                if (!isMdUp) {
                                    handleDrawerToggle();
                                }
                            }}
                            sx={listItemStyles}
                        >
                            <ListItemIcon sx={{ color: isActive ? 'primary.main' : 'text.secondary' }}>
                                {item.icon}
                            </ListItemIcon>
                            <ListItemText primary={item.text} />
                        </ListItemButton>
                    );
                })}
            </List>
        </div>
    );
    
    return (
        <Box
            component="nav"
            sx={{ width: { md: drawerWidth }, flexShrink: { md: 0 } }}
            aria-label="main navigation"
        >
            <Drawer
                variant="temporary"
                open={tabletOpen}
                onClose={handleDrawerToggle}
                ModalProps={{
                    keepMounted: true,
                }}
                    sx={{
                    display: { xs: 'block', md: 'none' },
                    '& .MuiDrawer-paper': {
                        boxSizing: 'border-box',
                        width:  drawerWidth,
                        borderRight: 'none',
                    }
                }}
            >
                {drawerContent}
            </Drawer>
            <Drawer
                variant="permanent"
                sx={{
                    display: { xs: 'none', md: 'block' }, // Show only on sm and up
                    '& .MuiDrawer-paper': {
                        boxSizing: 'border-box',
                        width: drawerWidth,
                        borderRight: 'none'
                    },
                }}
                open
            >
                {drawerContent}
            </Drawer>
        </Box>
    )
};

export default Sidebar;