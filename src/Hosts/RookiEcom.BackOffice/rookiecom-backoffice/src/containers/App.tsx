import { AccessDeniedPage, HomePage, NotFoundPage, DashboardPage } from '../pages';
import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import SilentRenew from '../oidc/SilentRenew';
import { ThemeProvider } from '@emotion/react';
import theme from '../theme';
import { useAuth } from 'react-oidc-context';
import { useEffect } from 'react';
import { Box, CircularProgress, Typography } from '@mui/material';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { MiniLoaderPage } from '../components/common';
import { ProductCreatePage, ProductListPage } from '../pages/Product';
import { CategoryCreatePage, CategoryListPage } from '../pages/Category';


const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      staleTime: 1000 * 60,
    },
  },
});

const LogoutCallback = () => {
  const auth = useAuth();

  useEffect(() => {
    auth.removeUser().then(() => {
      window.location.replace("/");
    });
  }, [auth]);

  return (
    <MiniLoaderPage text="Logging out..." />
  );
};
const App = () => {


  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <BrowserRouter>
          <Routes>
            <Route path="/signin-oidc" element={<SilentRenew />} />
            <Route path="/silent-renew" element={<SilentRenew />} />
            <Route path="/logout-callback" element={<LogoutCallback />} />
            <Route path="/">
              <Route index element={<DashboardPage />} />
              <Route path="products">
                <Route index element={<ProductListPage />} />
                {/* <Route path=":id" element={<ProductDetailPage />} /> */}
                <Route path="create" element={<ProductCreatePage />} />
                {/* <Route path="/edit/:id" element={<ProductEditPage />} /> */}
              </Route>
              <Route path="categories">
                <Route index element={<CategoryListPage />} />
                {/* <Route path=":id" element={<CategoryDetailPage />} /> */}
                <Route path="create" element={<CategoryCreatePage />} />
                {/* <Route path="/edit/:id" element={<CategoryEditPage />} /> */}
              </Route>
            </Route>
            <Route path="/access-denied" element={<AccessDeniedPage />} />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </BrowserRouter>
      </ThemeProvider>
    </QueryClientProvider>
  );
};

export default App;
