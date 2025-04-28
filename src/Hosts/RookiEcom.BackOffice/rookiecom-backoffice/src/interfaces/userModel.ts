import IAddress from "./valueObjects/address";

export default interface IUserModel {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    phoneNumber?: string;
    doB: string;
    avatar?: string;
    address?: IAddress
};