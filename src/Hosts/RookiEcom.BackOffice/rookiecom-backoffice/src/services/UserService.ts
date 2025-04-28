import ApiWebClient from "../apis/apiClient";
import { IApiResponse, IUserModel } from "../interfaces";
import { PagedResult } from "../interfaces/pagedResult";

export class UserService {
    private readonly client: ApiWebClient;

    public constructor(client: ApiWebClient) {
        this.client = client;
    }
    
    public async getCustomersPagings(pageNumber: number = 1, pageSize: number = 10): Promise<PagedResult<IUserModel>> {
        const response = await this.client.get(`/api/v1/users?pageNumber=${pageNumber}&pageSize=${pageSize}`) as IApiResponse;

        return response.result as PagedResult<IUserModel>;
    }

    public async getProfile(userId: string): Promise<IUserModel> {
        const response = await this.client.get(`/api/v1/users/${userId}`) as IApiResponse;
        
        return response.result as IUserModel;
    }   
}