export default interface IApiResponse {
  result?: any;
  statusCode?: number;
  isSuccess?: boolean;
  errorMessages?: Array<string>;
}

