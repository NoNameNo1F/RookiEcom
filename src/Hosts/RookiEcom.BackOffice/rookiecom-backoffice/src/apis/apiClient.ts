import { useAuth } from 'react-oidc-context';

const API_BASE_URL = import.meta.env.VITE_HTTPS_WEBAPI_URL as string;
const API_IDP_URL = import.meta.env.VITE_HTTPS_OAUTH_URL as string;

export const apiWebClient = async (endpoint: string, options: RequestInit = {}) => {
  const auth = useAuth();
  const token = auth.user?.access_token;

  const headers = new Headers(options.headers || {});
  headers.set('Content-Type', 'application/json');
  if (token) {
    headers.set('Authorization', `Bearer ${token}`);
  }

  const response = await fetch(`${API_BASE_URL}${endpoint}`, {
    ...options,
    headers,
  });

  if (!response.ok) {
    throw new Error(`API error: ${response.statusText}`);
  }

  return response.json();
};