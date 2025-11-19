import apiClient from "./apiClient";
import { setAccessToken } from "../tokenStore";

export async function login(
  username: string,
  password: string,
): Promise<string | null> {
  const { data } = await apiClient.post("/auth/login", {
    username,
    password,
  });
  const { accessToken } = data;
  return accessToken || null;
}

export async function logout(): Promise<void> {
  await apiClient.post("/auth/logout");
}

export async function refreshToken() {
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
}
