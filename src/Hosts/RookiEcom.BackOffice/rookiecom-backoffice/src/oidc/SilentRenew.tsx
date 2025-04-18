import { useEffect } from "react";
import { useAuth } from "react-oidc-context";
import { useNavigate } from "react-router-dom";
import { Box, CircularProgress, Typography } from "@mui/material";

const SilentRenew = () => {
    const auth = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (window.location.pathname === "/signin-oidc") {
            if (auth.isAuthenticated && !auth.isLoading) {
                console.log("Sign-in complete, redirecting to /dashboard");
                navigate("/dashboard", { replace: true });
            } else if (auth.error) {
                console.error("Sign-in error:", auth.error);
                navigate("/", { replace: true });
            }
        }
        // Handle silent renew (/silent-renew)
        if (window.location.pathname === "/silent-renew") {
            auth.signinSilent().catch((error) => {
                console.error("Silent renew error:", error);
            });
        }
    }, [auth, navigate]);

    return (
        <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "100vh" }}>
            <CircularProgress />
            <Typography sx={{ ml: 2 }}>Processing authentication...</Typography>
        </Box>
    );
};

export default SilentRenew;