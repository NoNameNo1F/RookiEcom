import { AccessDeniedPage, NotFoundPage, DashboardPage } from '../pages';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import SilentRenew from '../oidc/SilentRenew';
import { CreateProductPage, EditProductPage, ProductListPage } from '../pages/Product';
import { CategoryCreatePage, CategoryListPage, EditCategoryPage } from '../pages/Category';
import { LogoutCallback } from '../oidc/LogoutCallback';
import { DashboardLayout } from '../components';

const App: React.FC = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/signin-oidc" element={<SilentRenew />} />
                <Route element={<DashboardLayout />} >
                    <Route path="/">
                        <Route index element={<DashboardPage />} />
                        <Route path="products">
                        <Route index element={<ProductListPage />} />
                        <Route path="create" element={<CreateProductPage />} />
                        <Route path="edit/:sku" element={<EditProductPage />} />
                        </Route>
                        <Route path="categories">
                        <Route index element={<CategoryListPage />} />
                        <Route path="create" element={<CategoryCreatePage />} />
                        <Route path="edit/:id" element={<EditCategoryPage />} />
                        </Route>
                    </Route>
                </Route>
                <Route path="/silent-renew" element={<SilentRenew />} />
                <Route path="/logout-callback" element={<LogoutCallback />} />
                <Route path="/access-denied" element={<AccessDeniedPage />} />
                <Route path="*" element={<NotFoundPage />} />
            </Routes>
        </BrowserRouter>            
    );
};

export default App;
