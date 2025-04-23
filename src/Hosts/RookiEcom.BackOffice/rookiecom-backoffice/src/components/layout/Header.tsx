import { AppBar, Button, IconButton, Toolbar, Typography, useTheme } from "@mui/material";
import Brightness4Icon from '@mui/icons-material/Brightness4';
import Brightness7Icon from '@mui/icons-material/Brightness7';
import { useAuth } from "react-oidc-context";
import { ThemeToggleContext } from "../../"
import { useContext } from "react";

export const Header = () => {
    const auth = useAuth();
    const { toggleTheme } = useContext(ThemeToggleContext);
    const theme = useTheme();

    const handleLogout = () => {
        auth.signoutRedirect();
    };

    return (
        <AppBar
            position="fixed"
            sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
            color="primary">
            <Toolbar>
                <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1 }}>
                    RookiEcom BackOffice
                </Typography>
                <IconButton onClick={toggleTheme} color="inherit">
                    {theme.palette.mode === 'dark' ? <Brightness7Icon /> : <Brightness4Icon />}
                    </IconButton>
                <Button variant="contained" color="error" onClick={handleLogout}>
                    Sign Out
                </Button>
            </Toolbar>
        </AppBar>
    );
};
