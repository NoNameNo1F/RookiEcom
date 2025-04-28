import { useQuery } from "@tanstack/react-query";
import { UserService } from "../services";
import { idpClient } from "../apis/apiClient";

const userService = new UserService(idpClient);
export const useGetCustomers = (pageNumber: number = 1, pageSize: number = 25) => {
    return useQuery({
        queryKey: ['users', pageNumber, pageSize],
        queryFn: () =>
            userService.getCustomersPagings(pageNumber, pageSize),
    });
};

export const useGetProfile = (userId: string) => {
    return useQuery({
        queryKey: ['user', userId],
        queryFn: () =>
            userService.getProfile(userId),
        enabled: !!userId,
        staleTime: 5 * 60 * 1000,
    });
};