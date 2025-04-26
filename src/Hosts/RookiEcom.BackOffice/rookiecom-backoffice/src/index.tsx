import React, { createContext, useMemo, useState } from 'react';
import ReactDOM from 'react-dom/client';
import App from './containers/App.tsx';
import './index.css';
import { AuthProvider } from 'react-oidc-context';
import { ToastContainer } from 'react-toastify';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ThemeProvider } from '@emotion/react';
import { CssBaseline } from '@mui/material';
import { getTheme } from './theme.ts';
import userManager from './oidc/oidcConfig.ts';

export const ThemeToggleContext = createContext<{ toggleTheme: () => void }>({ toggleTheme: () => {} });

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      staleTime: 1000 * 60,
    },
  },
});

const AppContext: React.FC = () => {
    const [mode, setMode] = useState<'light' | 'dark'>('light');

    const theme = useMemo(() => getTheme(mode)
        , [mode]);

    const toggleTheme = () => {
        setMode((prevMode) => (prevMode === 'light' ? 'dark' : 'light'));
    };
    
    return (
        <ThemeToggleContext.Provider value={{ toggleTheme }}>
            <ThemeProvider theme={theme}>
                <CssBaseline />
                <QueryClientProvider client={queryClient}>
                    <AuthProvider userManager={userManager}>
                        <ToastContainer
                            position="top-right"
                            autoClose={3000}
                            hideProgressBar={false}
                            newestOnTop={false}
                            closeOnClick
                            rtl={false}
                            pauseOnFocusLoss
                            draggable
                            pauseOnHover
                            theme="colored"
                        />
                        <App />
                    </AuthProvider>
                </QueryClientProvider>
            </ThemeProvider>
        </ThemeToggleContext.Provider>
    );
};

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <AppContext />
  </React.StrictMode>
);
