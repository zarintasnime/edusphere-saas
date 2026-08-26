import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from 'react';

import { api, setUnauthorizedHandler, TOKEN_KEY } from '../lib/api';
import type { AuthResponse, RoleType, UserResponse } from '../lib/types';

interface SessionUser {
  userId: number;
  institutionId: number;
  fullName: string;
  email: string;
  role: RoleType;
}

interface AuthState {
  user: SessionUser | null;
  status: 'loading' | 'authenticated' | 'anonymous';
  signIn: (email: string, password: string) => Promise<SessionUser>;
  signOut: () => void;
}

const AuthContext = createContext<AuthState | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<SessionUser | null>(null);
  const [status, setStatus] = useState<AuthState['status']>('loading');

  const signOut = useCallback(() => {
    localStorage.removeItem(TOKEN_KEY);
    setUser(null);
    setStatus('anonymous');
  }, []);

  // A 401 from any request means the token is gone or expired.
  useEffect(() => {
    setUnauthorizedHandler(signOut);
  }, [signOut]);

  /**
   * On a page refresh the token is all that survives, so the session is rebuilt
   * from the API rather than from a cached user object that could be stale.
   */
  useEffect(() => {
    const token = localStorage.getItem(TOKEN_KEY);

    if (!token) {
      setStatus('anonymous');
      return;
    }

    let cancelled = false;

    api
      .get<UserResponse>('/api/Auth/me')
      .then(({ data }) => {
        if (cancelled) return;

        setUser({
          userId: data.userId,
          institutionId: data.institutionId ?? 0,
          fullName: data.fullName,
          email: data.email,
          role: data.role,
        });
        setStatus('authenticated');
      })
      .catch(() => {
        if (cancelled) return;
        localStorage.removeItem(TOKEN_KEY);
        setStatus('anonymous');
      });

    return () => {
      cancelled = true;
    };
  }, []);

  const signIn = useCallback(async (email: string, password: string) => {
    const { data } = await api.post<AuthResponse>('/api/Auth/login', {
      email,
      password,
    });

    localStorage.setItem(TOKEN_KEY, data.token);

    const session: SessionUser = {
      userId: data.userId,
      institutionId: data.institutionId ?? 0,
      fullName: data.fullName,
      email: data.email,
      role: data.role,
    };

    setUser(session);
    setStatus('authenticated');

    return session;
  }, []);

  const value = useMemo(
    () => ({ user, status, signIn, signOut }),
    [user, status, signIn, signOut],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error('useAuth must be used inside <AuthProvider>.');
  }

  return context;
}

/** Where each role lands after signing in. */
export function homeRouteFor(role: RoleType): string {
  if (role === 'Teacher') return '/teacher';
  if (role === 'Student') return '/student';
  return '/admin';
}
