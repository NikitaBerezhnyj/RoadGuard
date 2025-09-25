export interface ILoginRequest {
  email: string;
  password: string;
}

export interface IRegisterRequest {
  login: string;
  password: string;
  username: string;
  carMake?: string;
  carColor?: string;
  isAnonymous: boolean;
}

export interface IAuthResponse {
  token: string;
  userId: number;
}
