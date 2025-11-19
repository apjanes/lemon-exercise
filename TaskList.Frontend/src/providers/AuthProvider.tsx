import React, {
  createContext,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react";
import { setAccessToken } from "~/tokenStore";
import {
  login as apiLogin,
  logout as apiLogout,
  refreshToken as apiRefresh,
} from "~/api/auth";

interface AuthContextType {
  accessToken: string | null;
  isAuthenticated: boolean;
  isInitializing?: boolean;
  login: (username: string, password: string) => Promise<boolean>;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  // DEBUG: possibly move
  const [accessTokenState, setAccessTokenState] = useState<string | null>(null);
  const [isInitializing, setIsInitializing] = useState<boolean>(true);

  useEffect(() => {
    setIsInitializing(true);

    (async () => {
      try {
        const token = await apiRefresh();
        setAccessTokenState(token);
      } finally {
        setIsInitializing(false);
      }
    })();
  }, []);

  const isAuthenticated = useMemo(() => {
    return accessTokenState !== null;
  }, [accessTokenState, isInitializing]);

  const login = async (username: string, password: string) => {
    const token = await apiLogin(username, password);
    setAccessToken(token);
    setAccessTokenState(token);
    return !!token;
  };

  const logout = async () => {
    await apiLogout();
    setAccessToken(null);
    setAccessTokenState(null);
  };

  return (
    <AuthContext.Provider
      value={{
        accessToken: accessTokenState,
        isAuthenticated: isAuthenticated,
        isInitializing,
        login,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
