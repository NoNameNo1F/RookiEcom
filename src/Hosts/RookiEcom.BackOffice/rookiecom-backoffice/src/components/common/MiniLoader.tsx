import { Box, CircularProgress, Typography } from "@mui/material";

function MiniLoaderPage({ text = "" }) {
  return (
    <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", minHeight: "100vh" }}>
      <CircularProgress />
      <Typography sx={{ ml: 2 }}>{text}</Typography>
    </Box>
  );
}

export default MiniLoaderPage;
