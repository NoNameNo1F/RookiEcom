import { Box, Typography } from "@mui/material";

export default function AccessDeniedPage() {
    return (
    <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
      <Typography variant="h4" color="error">
        Access Denied
      </Typography>
    </Box>
  );
};
