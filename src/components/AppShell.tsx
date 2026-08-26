import { useState } from 'react';
import { NavLink, Outlet } from 'react-router-dom';
import { Camera, LogOut, Menu, X } from 'lucide-react';

import { useAuth } from '../auth/AuthContext';
import { cx } from '../lib/format';
import { Avatar } from './Avatar';
import { EditAvatarModal } from './EditAvatarModal';
import { NotificationBell } from './NotificationBell';

export interface NavItem {
  to: string;
  label: string;
  end?: boolean;
}

const roleLabel: Record<string, string> = {
  SuperAdmin: 'Super admin',
  Admin: 'Administrator',
  Teacher: 'Teacher',
  Student: 'Student',
};

export function AppShell({ nav, homeRoute }: { nav: NavItem[]; homeRoute: string }) {
  const { user, signOut } = useAuth();
  const [menuOpen, setMenuOpen] = useState(false);
  const [editAvatarOpen, setEditAvatarOpen] = useState(false);

  /* Nav items are numbered like a ledger index. The active item gets a solid
     acid bar rather than a filled pill, so the sidebar stays quiet. */
  const links = (
    <nav className="space-y-0.5">
      {nav.map((item, index) => (
        <NavLink
          key={item.to}
          to={item.to}
          end={item.end}
          onClick={() => setMenuOpen(false)}
          className={({ isActive }) =>
            cx(
              'group flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm transition-colors',
              isActive
                ? 'bg-white/[0.07] text-paper'
                : 'text-paper/55 hover:bg-white/[0.04] hover:text-paper',
            )
          }
        >
          {({ isActive }) => (
            <>
              <span
                aria-hidden
                className={cx(
                  'h-4 w-[3px] rounded-full transition-colors',
                  isActive ? 'bg-acid' : 'bg-transparent group-hover:bg-paper/25',
                )}
              />
              <span className="font-mono text-[10px] text-paper/30">
                {String(index + 1).padStart(2, '0')}
              </span>
              {item.label}
            </>
          )}
        </NavLink>
      ))}
    </nav>
  );

  return (
    <div className="min-h-screen lg:flex">
      {/* Mobile bar */}
      <header className="sticky top-0 z-40 flex items-center justify-between border-b border-ink/10 bg-ink px-4 py-3 lg:hidden">
        <Wordmark />
        <div className="flex items-center gap-1">
          <NotificationBell homeRoute={homeRoute} />
          <button
            type="button"
            onClick={() => setMenuOpen((open) => !open)}
            aria-label={menuOpen ? 'Close menu' : 'Open menu'}
            className="rounded-lg p-2 text-paper/70 hover:bg-white/10 hover:text-paper"
          >
            {menuOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}
          </button>
        </div>
      </header>

      {menuOpen && (
        <div className="border-b border-ink/10 bg-ink px-4 pb-4 lg:hidden">
          {links}
          <button
            type="button"
            onClick={signOut}
            className="mt-2 flex w-full items-center gap-2 rounded-lg px-3 py-2.5 text-sm text-paper/55 hover:bg-white/[0.04] hover:text-paper"
          >
            <LogOut className="h-4 w-4" />
            Sign out
          </button>
        </div>
      )}

      {/* Sidebar */}
      <aside className="sticky top-0 hidden h-screen w-64 shrink-0 flex-col justify-between bg-ink px-4 py-6 lg:flex">
        <div className="space-y-8">
          <div className="px-1">
            <Wordmark />
          </div>
          {links}
        </div>

        <div className="space-y-3 border-t border-white/10 pt-4">
          <div className="flex items-center gap-3 px-1">
            <button
              type="button"
              onClick={() => setEditAvatarOpen(true)}
              title="Change profile picture"
              aria-label="Change profile picture"
              className="group relative rounded-full focus:outline-none focus:ring-2 focus:ring-acid/80 focus:ring-offset-2 focus:ring-offset-ink"
            >
              <Avatar email={user?.email} name={user?.fullName} id={user?.userId} size="md" />
              <div className="absolute inset-0 flex items-center justify-center rounded-full bg-ink/75 text-acid opacity-0 transition-opacity group-hover:opacity-100">
                <Camera className="h-4 w-4" />
              </div>
            </button>
            <div className="min-w-0">
              <p className="truncate text-sm text-paper">{user?.fullName}</p>
              <p className="font-mono text-[10px] uppercase tracking-[0.14em] text-paper/40">
                {roleLabel[user?.role ?? ''] ?? user?.role}
              </p>
            </div>
          </div>

          <button
            type="button"
            onClick={signOut}
            className="flex w-full items-center gap-2 rounded-lg px-3 py-2 text-sm text-paper/55 transition-colors hover:bg-white/[0.06] hover:text-paper"
          >
            <LogOut className="h-4 w-4" />
            Sign out
          </button>
        </div>
      </aside>

      <div className="min-w-0 flex-1">
        {/* Desktop top strip: keeps the bell reachable without stealing sidebar space */}
        <div className="hidden items-center justify-end gap-3 border-b border-rule bg-ink px-8 py-2.5 lg:flex">
          <NotificationBell homeRoute={homeRoute} />
          <button
            type="button"
            onClick={() => setEditAvatarOpen(true)}
            title="Change profile picture"
            aria-label="Change profile picture"
            className="group relative rounded-full focus:outline-none focus:ring-2 focus:ring-acid/80"
          >
            <Avatar email={user?.email} name={user?.fullName} id={user?.userId} size="sm" />
            <div className="absolute inset-0 flex items-center justify-center rounded-full bg-ink/75 text-acid opacity-0 transition-opacity group-hover:opacity-100">
              <Camera className="h-3 w-3" />
            </div>
          </button>
        </div>

        <main className="px-4 py-7 sm:px-8 sm:py-10">
          <div className="mx-auto max-w-5xl">
            <Outlet />
          </div>
        </main>
      </div>

      <EditAvatarModal
        open={editAvatarOpen}
        onClose={() => setEditAvatarOpen(false)}
        email={user?.email}
        name={user?.fullName}
        id={user?.userId}
      />
    </div>
  );
}

export function Wordmark({ tone = 'light' }: { tone?: 'light' | 'dark' }) {
  return (
    <div className="flex items-center gap-2.5">
      {/* Sleek minimalist logo: a rounded container with letter 'E' and an emerald accent dot */}
      <span
        aria-hidden
        className={cx(
          'relative flex h-7 w-7 items-center justify-center rounded-lg font-display text-sm font-bold shadow-sm',
          tone === 'light' ? 'bg-paper text-ink' : 'bg-ink text-paper',
        )}
      >
        <span>E</span>
        <span
          className="absolute -right-0.5 -top-0.5 h-2.5 w-2.5 rounded-full bg-emerald-500 ring-2 ring-ink"
        />
      </span>
      <span
        className={cx(
          'font-display text-lg font-semibold tracking-tight',
          tone === 'light' ? 'text-paper' : 'text-ink',
        )}
      >
        Edu<span className="text-emerald-400 font-bold">Sphere</span>
      </span>
    </div>
  );
}
