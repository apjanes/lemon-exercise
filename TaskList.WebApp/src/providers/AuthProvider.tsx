import React, { createContext, useContext, useEffect, useState } from "react";
import { getAccessToken, setAccessToken } from "~/tokenStore";
import auth from "~/api/auth";

interface AuthContextType {
  accessToken: string | null;
  isAuthenticated: () => boolean;
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
        const token = await auth.refreshToken();
        setAccessTokenState(token);
      } finally {
        setIsInitializing(false);
      }
    })();
  }, []);

  const isAuthenticated = () => !!getAccessToken();

  const login = async (username: string, password: string) => {
    const token = await auth.login(username, password);
    setAccessToken(token);
    setAccessTokenState(token);
    return !!token;
  };

  const logout = async () => {
    await auth.logout();
    setAccessToken(null);
    setAccessTokenState(null);
  };

  return (
    <AuthContext.Provider
      value={{
        accessToken: accessTokenState,
        isAuthenticated,
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
