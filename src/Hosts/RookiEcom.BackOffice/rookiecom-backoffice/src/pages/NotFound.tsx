import { Box, Typography } from "@mui/material";

export default function NotFoundPage() {
    return (
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
            <Typography variant="h4">
                404 - Page Not Found
            </Typography>
        </Box>
    );
};
