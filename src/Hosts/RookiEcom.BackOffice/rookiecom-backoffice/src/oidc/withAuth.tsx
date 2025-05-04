import { useAuth, withAuthenticationRequired, } from "react-oidc-context";
import { jwtDecode } from "jwt-decode";
import React, { useEffect } from 'react';
import { useNavigate } from "react-router-dom";
import { IJwtPayloadModel } from "../interfaces";
import { MiniLoaderPage } from "../components/common";

const withAuth = <P extends object>(
    WrappedComponent: React.FC<P>
) => {
    const AuthComponent = withAuthenticationRequired(
        (props: P) => {
            const auth = useAuth();
            const navigate = useNavigate();

            const decodedToken = jwtDecode<IJwtPayloadModel>(auth.user?.access_token!);
            const roles = decodedToken?.roles!;
            useEffect(() => {
                if (!auth.isAuthenticated){
                    navigate("/login", { replace: true});
                } else if (!roles.includes("Admin")) {
                    navigate("access-denied", { replace: true });
                }
            }, [auth.isAuthenticated, roles, navigate]);

            if (auth.isLoading || !auth.isAuthenticated) {
                return null;
            }

            return <WrappedComponent {...props} />;
        },
        {
            OnRedirecting: () => <MiniLoaderPage text="Redirecting to login..." />,
        }
    );
    return AuthComponent;
};

export default withAuth;