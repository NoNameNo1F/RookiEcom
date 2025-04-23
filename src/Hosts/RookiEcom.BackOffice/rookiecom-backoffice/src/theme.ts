import { createTheme } from '@mui/material/styles';
import { PaletteMode } from '@mui/material';

export const getTheme = (mode: PaletteMode) => {
  return createTheme({
    palette: {
      mode,
      primary: {
        light: mode === 'light' ? '#010410' : '#4b5eAA',
        main: mode === 'light' ? '#020617' : '#647ACB',
        dark: mode === 'light' ? '#343745' : '#3E55A2',
        contrastText: '#f8fafc',
      },
      secondary: {
        light: mode === 'light' ? '#afafaf' : '#d9d9d9',
        main: mode === 'light' ? '#fafafa' : '#e6e6e6',
        dark: mode === 'light' ? '#fbfbfb' : '#cccccc',
        contrastText: '#fbfbfb',
      },
      background: {
        default: mode === 'light' ? '#f8f9fa' : '#121212',
        paper: mode === 'light' ? '#ffffff' : '#1e1e1e',
      },
    },
    components: {
      MuiCard: {
        styleOverrides: {
          root: {
            borderRadius: 12,
          },
        },
      },
    },
  });
};