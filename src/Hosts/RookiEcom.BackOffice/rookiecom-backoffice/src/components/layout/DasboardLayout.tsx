import React, { useState } from 'react';
import { Box, CssBaseline, Toolbar, useMediaQuery, useTheme } from '@mui/material';
import { Header } from './Header';
import Sidebar from './Sidebar';
import { Outlet } from 'react-router-dom';

const drawerWidth = 240;
const DashboardLayout: React.FC = () => {
    const theme = useTheme();

    const [tabletOpen, setTabletOpen] = useState(false);

    const handleDrawerToggle = () => {
    setTabletOpen(!tabletOpen);
  };
  return (
      <Box sx={{ display: 'flex', minHeight: '100vh' }}>
        <CssBaseline />
          <Header
              drawerWidth={drawerWidth}
              handleDrawerToggle={handleDrawerToggle}
          />
          <Sidebar
              drawerWidth={drawerWidth}
                tabletOpen={tabletOpen}
              handleDrawerToggle={handleDrawerToggle}
          />
        <Box
            component="main"
            sx={{
                flexGrow: 1,
                p: 3,
                mt: {  
                    xs: '56px',
                    sm: '64px'
                },
                width: { sm: `calc(100% - ${drawerWidth}px)` },
                // ml: `${theme.mixins.toolbar.minHeight}px`,
                overflowX: 'hidden',
                boxShadow: '0px 4px 10px rgba(0, 0, 0, 0.1)',
                // [theme.breakpoints.up('md')]: {
                //     mt: '64px',
                //     marginLeft: `${drawerWidth}px`,
                // },
                // [theme.breakpoints.down('md')]: { 
                //     mt: '56px',
                // }
            }}
        >
            <Outlet />
        </Box>
    </Box>
  );
};

export default DashboardLayout;