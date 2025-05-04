import { useEffect } from "react";
import { useAuth } from "react-oidc-context";
import { useNavigate } from "react-router-dom";
import { MiniLoaderPage } from "../components/common";

const SilentRenew: React.FC  = () => {
    const auth = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (window.location.pathname === "/signin-oidc") {
            if (auth.isAuthenticated && !auth.isLoading) {
                navigate("/", { replace: true });
            } else if (auth.error) {
                console.error("Sign-in error:", auth.error);
                navigate("/", { replace: true });
            }
        }
        if (window.location.pathname === "/silent-renew" && auth.isAuthenticated) {
            auth.signinSilent().catch((error) => {
                console.error("Silent renew error:", error);
            });
        }
    }, [auth, navigate]);

    return (
        <MiniLoaderPage text={"Processing authentication..."} />
    );
};

export default SilentRenew;