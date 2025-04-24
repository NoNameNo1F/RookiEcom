import { Box, Typography } from "@mui/material";
import { useAuth } from "react-oidc-context";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import withAuth from "../../oidc/withAuth";
// import { IJwtPayloadModel } from "../interfaces";
// import withAuth from "../oidc/withAuth";

const CustomerListPage = () => {
    return (
        <Box sx={{ display: 'flex', minHeight: '100vh' }}>
            <Box component="main" sx={{ flexGrow: 1, p: 3, backgroundColor: '#f8f9fa' }}>
                <Typography variant="h4" gutterBottom>
                    Customers
                </Typography>
                <Typography variant="body1">
                    Placeholder for users list. No API endpoint provided. Admin access only.
                </Typography>
            </Box>
        </Box>
    );
};

export default withAuth(CustomerListPage);