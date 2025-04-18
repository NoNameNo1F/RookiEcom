import { useAuth, withAuthenticationRequired, } from "react-oidc-context";

import React, { useEffect } from 'react';
import { useNavigate } from "react-router-dom";

const withAuth = <P extends object>(
    WrappedComponent: React.ComponentType<P>
) => {
    const AuthComponent = withAuthenticationRequired(
        (props: P) => {
            const auth = useAuth();
            const navigate = useNavigate();

            const roles = Array.isArray(auth.user!.profile!.roles)
                ? auth.user?.profile.roles
                : typeof auth.user?.profile.roles === "string"
                    ? [auth.user?.profile.roles] : [];

            useEffect(() => {


                if (!roles?.includes("Admin")) {
                    navigate("/", { replace: true });
                }
            }, [roles, navigate]);

            if (auth.isLoading || !auth.isAuthenticated) {
                return null;
            }

            return <WrappedComponent {...props} />;
        }
    );
    return AuthComponent;
};

export default withAuth;