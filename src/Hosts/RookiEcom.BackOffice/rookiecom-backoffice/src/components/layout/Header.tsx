import { AppBar, Button, Toolbar, Typography } from "@mui/material";
import { useAuth } from "react-oidc-context";

export default function Header() {
    const auth = useAuth();
    
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
                <Button variant="contained" color="error" onClick={handleLogout}>
                    Sign Out
                </Button>
            </Toolbar>
        </AppBar>
    );
};
