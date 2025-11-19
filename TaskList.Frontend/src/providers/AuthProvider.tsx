import React, { createContext, useContext, useEffect, useState } from "react";
import { getAccessToken } from "~/tokenStore";
import { login as apiLogin, logout as apiLogout, refreshToken } from "~/auth";

interface AuthContextType {
  accessToken: string | null;
  isAuthenticated: boolean;
  isInitializing?: boolean;
  userLogin: (username: string, password: string) => Promise<void>;
  userLogout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [accessTokenState, setAccessTokenState] = useState<string | null>(null);
  const [isInitializing, setIsInitializing] = useState<boolean>(true);

  useEffect(() => {
    setIsInitializing(true);

    (async () => {
      try {
        const token = await refreshToken();
        setAccessTokenState(token);
      } finally {
        setIsInitializing(false);
      }
    })();
  }, []);

  // useEffect(() => {
  //   setAccessToken(accessTokenState);
  // }, [accessTokenState]);

  const userLogin = async (username: string, password: string) => {
    await apiLogin(username, password);
    const token = getAccessToken();

    // DEBUG: remove?
    setAccessTokenState(token);
  };

  const userLogout = async () => {
    await apiLogout();
    // DEBUG: remove?
    setAccessTokenState(null);
  };

  return (
    <AuthContext.Provider
      value={{
        accessToken: accessTokenState,
        isAuthenticated: accessTokenState !== null,
        isInitializing,
        userLogin,
        userLogout,
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
