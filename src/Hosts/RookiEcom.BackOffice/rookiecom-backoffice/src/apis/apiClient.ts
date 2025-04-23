import axios, { AxiosInstance, AxiosRequestConfig, AxiosRequestHeaders } from 'axios';
import userManager from '../oidc/oidcConfig';
import { IApiResponse, IProblemDetails } from '../interfaces';

const API_BASE_URL = import.meta.env.VITE_HTTPS_WEBAPI_URL as string;
const API_IDP_URL = import.meta.env.VITE_HTTPS_OAUTH_URL as string;

export class ProblemDetailsError extends Error {
    public readonly problemDetails: IProblemDetails;

    public constructor(problemDetails: IProblemDetails) {
        super(problemDetails.detail);

        this.problemDetails = problemDetails;
    }
}

export default class ApiWebClient {
    private readonly axios: AxiosInstance;

    public constructor(baseURL: string) {
        this.axios = axios.create({
            baseURL: baseURL,
            headers: {
                "Content-Type": "application/json"
            }
        });

        this.axios.interceptors.request.use(async (config) => {
            const user = await userManager.getUser();
            
            if (user?.access_token) {
                config.headers = {
                    ...config.headers,
                    Authorization: `Bearer ${user.access_token}`
                } as AxiosRequestHeaders;
            }

            return config;
        });

        this.axios.interceptors.response.use((response) => response, async (error) => {
            if (error.response?.status === 401) {
                try {
                    await userManager.signinSilent();
                    const user = await userManager.getUser();
            
                    if (user?.access_token) {
                        error.config.headers.Authorization = `Bearer ${user.access_token}`;
                        return this.axios.request(error.config);
                    }
                } catch (refreshError) {
                    userManager.signinRedirect();
                }
            }

            console.error(error);

            throw new ProblemDetailsError(error.response.data);
        });
    };
    public async get(endpoint: string, config?: AxiosRequestConfig): Promise<IApiResponse> {
        const response = await this.axios.get(endpoint, config);
        return response.data;
    }

    public async post(endpoint: string, data: any, config?: AxiosRequestConfig): Promise<IApiResponse> {
        const response = await this.axios.post(endpoint, data, config);
        return response.data;
    }

    public async put(endpoint: string, data: any, config?: AxiosRequestConfig): Promise<IApiResponse> {
        const response = await this.axios.put(endpoint, data, config);
        return response.data;
    }

    public async delete(endpoint: string, config?: AxiosRequestConfig): Promise<IApiResponse> {
        const response = await this.axios.delete(endpoint, config);
        return response.data;
    }
}

const apiWebClient = new ApiWebClient(API_BASE_URL);
const idpClient = new ApiWebClient(API_IDP_URL);

export { apiWebClient, idpClient };