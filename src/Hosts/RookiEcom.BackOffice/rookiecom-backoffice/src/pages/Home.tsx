import { Button, Paper } from "@mui/material";
import { useAuth } from "react-oidc-context";
import DashboardPage from "./Dashboard";
import { useEffect } from 'react';
import { Box, Typography } from '@mui/material';
import withAuth from "../oidc/withAuth";
import { MiniLoaderPage } from "../components/common";


const HomePage = () => {
    const auth = useAuth();

    useEffect(() => {
        if (!auth.isAuthenticated && !auth.isLoading) {
            auth.signinRedirect();
        }
    }, [auth.isAuthenticated, auth.isLoading]);

    if (auth.isLoading) {
        return (<MiniLoaderPage text={"Loading..."} />);
    }

    if (!auth.isAuthenticated) {
        return null;
    }

    if (auth.error) {
        return <div>Oops... {auth.error.name} caused {auth.error.message}</div>;
    }

    const handleLogout = () => {
        auth.revokeTokens(["access_token", "refresh_token"]).then(() => {
            return auth.signoutRedirect({
                id_token_hint: auth.user?.id_token!,
                post_logout_redirect_uri: "https://localhost:3000/logout-callback",
            });
        }).then(() => {
            auth.removeUser();
        }).catch(error => {
            console.error("Logout error:", error);
        });
    };

    const userInfo = auth.user?.profile || {};

    return (
        <Box sx={{ maxWidth: 800, mx: "auto", mt: 4, p: 2 }}>
            <Typography variant="h4" gutterBottom>
                Welcome to RookiEcom BackOffice Home
            </Typography>
        </Box>
    );
};

export default withAuth(HomePage);