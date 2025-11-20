import apiClient from "./apiClient";
import { setAccessToken } from "../tokenStore";

const auth = {
  async login(username: string, password: string): Promise<string | null> {
    const { data } = await apiClient.post("/auth/login", {
      username,
      password,
    });
    const { accessToken } = data;
    return accessToken || null;
  },

  async logout(): Promise<void> {
    await apiClient.post("/auth/logout");
  },

  async refreshToken() {
    try {
      const { data } = await apiClient.post(
        "/auth/refresh",
        {},
        { withCredentials: true },
      );
      setAccessToken(data.accessToken);
      return data.accessToken;
    } catch {
      setAccessToken(null);
      return null;
    }
  },
};

export default auth;
