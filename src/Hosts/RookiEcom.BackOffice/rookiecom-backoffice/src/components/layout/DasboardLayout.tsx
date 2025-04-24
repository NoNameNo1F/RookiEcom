import React from 'react';
import { Box, Toolbar } from '@mui/material';
import { Header } from './Header';
import Sidebar from './Sidebar';
import { Outlet } from 'react-router-dom';

const DashboardLayout: React.FC = () => {
  return (
    <Box sx={{ display: 'flex', minHeight: '100vh' }}>
      <Header />
      <Sidebar />
      <Box
        component="main"
        sx={{
          flexGrow: 3,
          p: 3,
          backgroundColor: (theme) => theme.palette.background.default,
          width: { xs: '100%', sm: '85%' },
          boxShadow: '0px 4px 10px rgba(0, 0, 0, 0.1)'
        }}
      >
        <Toolbar />
        <Outlet />
      </Box>
    </Box>
  );
};

export default DashboardLayout;