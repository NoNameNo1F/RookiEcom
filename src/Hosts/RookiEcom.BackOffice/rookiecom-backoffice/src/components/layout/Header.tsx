import { AppBar, Box, Button, IconButton, Menu, MenuItem, Toolbar, Typography, useMediaQuery, useTheme } from "@mui/material";
import LightModeIcon from '@mui/icons-material/LightMode';
import DarkModeIcon from '@mui/icons-material/DarkMode';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import MenuIcon from '@mui/icons-material/Menu';
import { useAuth } from "react-oidc-context";
import { ThemeToggleContext } from "../../"
import { useContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { IJwtPayloadModel } from "../../interfaces";

interface HeaderProps {
    drawerWidth: number;
    handleDrawerToggle: () => void;
}

export const Header: React.FC<HeaderProps> = ({ drawerWidth, handleDrawerToggle}) => {
    const auth = useAuth();
    const navigate = useNavigate();

    const { toggleTheme } = useContext(ThemeToggleContext);
    const theme = useTheme();
    
    const decodedToken = auth.user?.access_token ? jwtDecode<IJwtPayloadModel>(auth.user.access_token) : null;
    const username = decodedToken?.name;

    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);

    const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    const handleLogout = () => {
        handleMenuClose();
        auth.signoutRedirect();
    };

    const handleProfileClick = () => {
        navigate('/profile');
        handleMenuClose();
    };

    return (
        <AppBar
            position='fixed'
            sx={{
                zIndex: (theme) => theme.zIndex.drawer + 1
            }}
            elevation={1}
        >
            <Toolbar>
                <IconButton
                    color="inherit"
                    aria-label="open drawer"
                    edge="start"
                    onClick={handleDrawerToggle}
                    sx={{ mr: 2, display: { md: 'none' } }}
                >
                    <MenuIcon />
                </IconButton>
                <Typography
                    variant="h6"
                    noWrap
                    component="div"
                    sx={{ flexGrow: 1 }}>
                    RookiEcom BackOffice
                </Typography>
                <Box sx={{display: 'flex', alignItems: 'center'}}>
                    <IconButton onClick={toggleTheme} color="inherit" sx={{ mr: 1 }}>
                        {theme.palette.mode === 'dark' ? <DarkModeIcon /> : <LightModeIcon />}
                    </IconButton>

                    <IconButton
                        edge="end"
                        color="inherit"
                        aria-label="account of current user"
                        aria-controls={open ? 'user-menu' : undefined}
                        aria-haspopup="true"
                        onClick={handleMenuOpen}
                    >
                        <AccountCircleIcon />
                    </IconButton>
                    <Menu
                        id="user-menu"
                        anchorEl={anchorEl}
                        open={open}
                        onClose={handleMenuClose}
                    
                         anchorOrigin={{
                           vertical: 'bottom',
                           horizontal: 'right',
                         }}
                         transformOrigin={{
                           vertical: 'top',
                           horizontal: 'right',
                         }}
                         sx={{ mt: 1 }}
                    >
                        {username && <MenuItem disabled sx={{ '&.Mui-disabled': { opacity: 1 } }}>
                           <Typography variant="body2">Hi, {username}</Typography>
                        </MenuItem>}
                        <MenuItem onClick={handleProfileClick}>Profile</MenuItem>
                        <MenuItem onClick={handleLogout}>Sign Out</MenuItem>
                    </Menu>
                </Box>
            </Toolbar>
        </AppBar>
    );
};
