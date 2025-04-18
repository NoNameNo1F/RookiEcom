import { AccessDeniedPage, HomePage, NotFoundPage, DashboardPage } from '../pages';
import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import SilentRenew from '../oidc/SilentRenew';
import { ThemeProvider } from '@emotion/react';
import theme from '../theme';
import ProtectedPage from '../oidc/ProtectedPage';

const App = () => {
  return (
    <ThemeProvider theme={theme}>
      <BrowserRouter>
        <Routes>
          {/* <Route path="/admin/login" element={<AdminLoginPage />} /> */}
          <Route path="/signin-oidc" element={<SilentRenew />} />
          <Route path="/silent-renew" element={<SilentRenew />} />
          {/* <Route element={<ProtectedPage />}> */}
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/" element={<HomePage />} />
          {/* </Route> */}
          <Route path="/access-denied" element={<AccessDeniedPage />} />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
};

export default App;
