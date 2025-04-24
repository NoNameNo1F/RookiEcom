import { UserManager, UserManagerSettings, WebStorageStateStore } from "oidc-client-ts";

const oidcConfig: UserManagerSettings = {
    authority: "https://localhost:8080",
    client_id: "rookiecom-backoffice",
    redirect_uri: "https://localhost:3000/signin-oidc",
    post_logout_redirect_uri: 'https://localhost:3000/logout-callback',
    response_type: "code",
    scope: "openid profile rookiecom-webapi role",
    automaticSilentRenew: false,
    silent_redirect_uri: 'https://localhost:3000/silent-renew',
    userStore: new WebStorageStateStore({ store: window.sessionStorage })
};

const userManager = new UserManager(oidcConfig);

export default userManager;