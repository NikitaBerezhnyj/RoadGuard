import apiClient from "@/utils/apiClient";
import { ILoginRequest, IRegisterRequest, IAuthResponse } from "@/interfaces/IAuth";

export const loginRequest = async (loginData: ILoginRequest): Promise<IAuthResponse> => {
  const response = await apiClient.post<IAuthResponse>("auth/login", loginData);
  return response.data;
};

export const registerRequest = async (registerData: IRegisterRequest): Promise<IAuthResponse> => {
  const response = await apiClient.post<IAuthResponse>("auth/register", registerData);
  return response.data;
};
