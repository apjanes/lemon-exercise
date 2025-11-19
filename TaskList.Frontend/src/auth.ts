import api from "./api";
import { getAccessToken, setAccessToken } from "./tokenStore";

export async function refreshToken() {
  try {
    const { data } = await api.post(
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

// DEBUG: remove
export async function getStatus() {
  // DEBUG: improve
  const { data } = await api.get("/auth/status");
  return data;
}
// DEBUG: move into AuthProvider later
export async function login(username: string, password: string) {
  const { data } = await api.post("/auth/login", { username, password });
  setAccessToken(data.accessToken);
}
// DEBUG: move into AuthProvider later
export async function logout() {
  await api.post("/auth/logout");
  setAccessToken(null);
}
