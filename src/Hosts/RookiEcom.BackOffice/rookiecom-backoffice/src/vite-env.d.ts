/// <reference types="vite/client" />
interface ImportMetaEnv {
  readonly VITE_HTTPS_OAUTH_URL: string;
  readonly VITE_HTTPS_WEBAPI_URL: string;
  // more env variables...
}

interface ImportMeta {
    readonly env: ImportMetaEnv;
}