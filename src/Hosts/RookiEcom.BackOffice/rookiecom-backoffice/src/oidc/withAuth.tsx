import { useAuth, withAuthenticationRequired, } from "react-oidc-context";
import { jwtDecode } from "jwt-decode";
import React, { useEffect } from 'react';
import { useNavigate } from "react-router-dom";
import { IJwtPayloadModel } from "../interfaces";

const withAuth = <P extends object>(
    WrappedComponent: React.ComponentType<P>
) => {
    const AuthComponent = withAuthenticationRequired(
        (props: P) => {
            const auth = useAuth();
            const navigate = useNavigate();

            const decodedToken = jwtDecode<IJwtPayloadModel>(auth.user?.access_token!);
            const role = decodedToken?.role!;

            useEffect(() => {
                if (role === "Admin") {
                    navigate("/", { replace: true });
                }
            }, [role, navigate]);

            if (auth.isLoading || !auth.isAuthenticated) {
                return null;
            }

            return <WrappedComponent {...props} />;
        }
    );
    return AuthComponent;
};

export default withAuth;