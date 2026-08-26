import { Navigate, useLocation } from 'react-router-dom';
import type { ReactNode } from 'react';

import { homeRouteFor, useAuth } from './AuthContext';
import type { RoleType } from '../lib/types';
import { FullPageSpinner } from '../components/ui';

interface Props {
  allow: RoleType[];
  children: ReactNode;
}

export function ProtectedRoute({ allow, children }: Props) {
  const { user, status } = useAuth();
  const location = useLocation();

  if (status === 'loading') {
    return <FullPageSpinner label="Restoring your session" />;
  }

  if (!user) {
    return <Navigate to="/login" replace state={{ from: location.pathname }} />;
  }

  // Signed in but on someone else's section: send them to their own home
  // instead of showing an error they cannot act on.
  if (!allow.includes(user.role)) {
    return <Navigate to={homeRouteFor(user.role)} replace />;
  }

  return <>{children}</>;
}
