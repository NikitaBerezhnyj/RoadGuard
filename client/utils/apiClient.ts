import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse, AxiosError } from "axios";

const API_BASE_URL = "http://localhost:5000/api/";

class ApiClient {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: API_BASE_URL,
      timeout: 10000,
      headers: {
        "Content-Type": "application/json"
      }
    });

    this.client.interceptors.request.use(
      (config: AxiosRequestConfig) => {
        return config;
      },
      (error: unknown) => Promise.reject(error)
    );

    this.client.interceptors.response.use(
      (response: AxiosResponse) => response.data,
      (error: AxiosError) => {
        if (error.response) {
          switch (error.response.status) {
            case 401:
              console.log("Unauthorized — можливо, потрібно перелогінитись");
              break;
            case 500:
              console.log("Серверна помилка, спробуйте пізніше");
              break;
            default:
              console.log("Сталася помилка:", error.response.status);
          }
        } else if (error.request) {
          console.log("Немає відповіді від сервера, перевірте підключення");
        } else {
          console.log("Помилка запиту:", error.message);
        }
        return Promise.reject(error);
      }
    );
  }

  get<T>(url: string, config?: AxiosRequestConfig) {
    return this.client.get<T>(url, config);
  }

  post<T>(url: string, data?: any, config?: AxiosRequestConfig) {
    return this.client.post<T>(url, data, config);
  }

  put<T>(url: string, data?: any, config?: AxiosRequestConfig) {
    return this.client.put<T>(url, data, config);
  }

  delete<T>(url: string, config?: AxiosRequestConfig) {
    return this.client.delete<T>(url, config);
  }
}

export default new ApiClient();
