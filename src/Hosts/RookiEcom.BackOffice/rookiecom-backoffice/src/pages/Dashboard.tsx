// import { Header } from "../components";

// export default function DashboardPage({
//     customers,
//     orders,
//     revenues,
//     products
// }: {
//     customers: React.ReactElement,
//     orders: React.ReactElement,
//     revenues: React.ReactElement,
//     products: React.ReactElement,
// }) {
//     return (
//         <div className="d-flex flex-column min-vh-100">
//             <Header />

//             <div className="container-fluid flex-grow-1 py-4">
//                 <div className="row">
//                     <aside className="col-md-3 d-none d-md-block bg-dark text-white p-3">
//                         <h2>Sidebar</h2>
//                         <ul className="nav flex-column">
//                             <li className="nav-item">
//                                 <a href="/" className="nav-link text-white">
//                                     <i className="bi bi-person-lines-fill"></i>
//                                     &nbsp; Dashboard
//                                 </a>
//                             </li>
//                             <li className="nav-item">
//                                 <a className="nav-link text-white" href="/orders">
//                                     <i className="bi bi-person-lines-fill"></i>
//                                     &nbsp; Orders
//                                 </a>
//                             </li>

//                             <li className="nav-item">
//                                 <a href="/products" className="nav-link text-white">
//                                     <i className="bi bi-person-lines-fill"></i>
//                                     Products
//                                 </a>
//                             </li>

//                             {/* {(user.role === "Admin") && ( */}
//                             <li className="nav-item">
//                                 <a href="/users" className="nav-link text-white">
//                                     <i className="bi bi-person-lines-fill"></i> &nbsp; Users
//                                 </a>
//                             </li>
//                             {/* )} > */}
//                         </ul>
//                     </aside>
//                     <main className="col-12 col-md-9">
//                         <div className="row mb-4">
//                             <div className="col-12 col-md-4 mb-3 mb-md-0">
//                                 {revenues}
//                             </div>
//                             <div className="col-12 col-md-4 mb-3 mb-md-0">
//                                 {orders}
//                             </div>
//                             <div className="col-12 col-md-4 mb-3 mb-md-0">
//                                 {customers}
//                             </div>
//                         </div>
//                         <div className="row">
//                             <div className="col-12">
//                                 {products}
//                             </div>
//                         </div>
//                     </main>

//                 </div>
//             </div>
//         </div>
//     );
// };

import { Box, Button, Typography } from "@mui/material";
import { useEffect } from "react";
import { useAuth } from "react-oidc-context";
import withAuth from "../oidc/withAuth";

const DashboardPage = () => {
    const auth = useAuth();

    const handleLogout = () => {
        auth.signoutRedirect();
    };

    return (
        <Box sx={{ maxWidth: 800, mx: "auto", mt: 4, p: 2 }}>
            <Typography variant="h4" gutterBottom>
                Welcome to RookiEcom BackOffice Dashboard
            </Typography>
            <Button
                variant="contained"
                color="error"
                onClick={handleLogout}
                sx={{ mt: 2 }}
            >
                Sign Out
            </Button>
        </Box>
    );
};

export default withAuth(DashboardPage);