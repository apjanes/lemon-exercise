import axios, { Axios, AxiosError } from "axios";
import { getAccessToken, setAccessToken } from "~/tokenStore";
import { refreshToken } from "./auth";

const baseURL = "https://localhost:4001";
// DEBUG: add to .env;
const apiClient = axios.create({
  baseURL,
  timeout: 10000,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

apiClient.interceptors.request.use((config) => {
  const token = getAccessToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

let isRefreshing = false;
let waiters: Array<() => void> = [];

apiClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const original = error.config!;
    const status = error.response?.status;

    if (status === 401 && !original.headers?.["x-retried"]) {
      if (!isRefreshing) {
        isRefreshing = true;
        try {
          await refreshToken();
          waiters.forEach((x) => x());
          waiters = [];
        } catch {
          setAccessToken(null);
          waiters = [];
        } finally {
          isRefreshing = false;
        }
      }

      await new Promise<void>((resolve) => waiters.push(resolve));

      original.headers.set("Authorization", `Bearer ${getAccessToken()}`);
      original.headers.set("x-retried", "1");

      return apiClient(original);
    }

    return Promise.reject(error);
  },
);

export default apiClient;
