import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    primary: {
      light: '#010410',
      main: '#020617',
      dark: '#343745',
      contrastText: '#f8fafc',
    },
    secondary: {
      light: '#afafaf',
      main: '#fafafa',
      dark: '#fbfbfb',
      contrastText: '#fbfbfb',
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

export default theme;