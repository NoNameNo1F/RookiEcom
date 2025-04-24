import { useEffect } from "react";
import { useAuth } from "react-oidc-context";
import { MiniLoaderPage } from "../components/common";

export const LogoutCallback: React.FC = () => {
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