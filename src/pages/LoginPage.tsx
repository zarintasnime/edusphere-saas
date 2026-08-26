import { useState, type FormEvent } from 'react';
import { Link, Navigate, useNavigate } from 'react-router-dom';
import { ArrowLeft, ArrowRight } from 'lucide-react';

import { homeRouteFor, useAuth } from '../auth/AuthContext';
import { errorMessage } from '../lib/api';
import { Wordmark } from '../components/AppShell';
import { Photo } from '../components/Photo';
import { Button, ErrorNote, Field, Input } from '../components/ui';
import { cx } from '../lib/format';

const demoAccounts = [
  {
    role: 'Administrator',
    email: 'admin@campusflow.dev',
    blurb: 'Departments, courses, people, enrolments',
  },
  {
    role: 'Teacher',
    email: 'teacher@campusflow.dev',
    blurb: 'Publish assignments, grade submissions',
  },
  {
    role: 'Student',
    email: 'student@campusflow.dev',
    blurb: 'Hand work in, read feedback',
  },
];

const DEMO_PASSWORD = 'Demo@123';

export default function LoginPage() {
  const { user, signIn } = useAuth();
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [picked, setPicked] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [busy, setBusy] = useState(false);

  if (user) {
    return <Navigate to={homeRouteFor(user.role)} replace />;
  }

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setError(null);
    setBusy(true);

    try {
      const session = await signIn(email.trim(), password);
      navigate(homeRouteFor(session.role), { replace: true });
    } catch (caught) {
      setError(errorMessage(caught));
    } finally {
      setBusy(false);
    }
  }

  function useDemo(demoEmail: string) {
    setEmail(demoEmail);
    setPassword(DEMO_PASSWORD);
    setPicked(demoEmail);
    setError(null);
  }

  return (
    <div className="min-h-screen lg:grid lg:grid-cols-[1fr_1.05fr]">
      {/* Left: the ledger panel. Dark, editorial, photo-anchored. */}
      <section className="relative hidden flex-col justify-between overflow-hidden bg-ink px-10 py-10 lg:flex">
        <div
          aria-hidden
          className="absolute inset-0 opacity-[0.06]"
          style={{
            backgroundImage:
              'repeating-linear-gradient(45deg, transparent 0 16px, #C8FF4D 16px 17px)',
          }}
        />

        <div className="relative">
          <Link to="/">
            <Wordmark />
          </Link>
        </div>

        <div className="relative max-w-sm">
          <Photo
            src="/images/campus.jpg"
            alt="Campus courtyard"
            label="Campus"
            ratio="aspect-[5/3]"
            className="mb-8 border border-white/10"
          />

          <p className="ledger-index text-acid">Welcome back</p>
          <h1 className="mt-3 font-display text-4xl font-semibold leading-tight text-paper">
            The work you set,
            <br />
            <span className="italic text-acid">and the work</span>
            <br />
            you get back.
          </h1>
        </div>

        <Link
          to="/"
          className="relative inline-flex items-center gap-2 font-mono text-xs uppercase tracking-[0.18em] text-paper/40 transition-colors hover:text-paper"
        >
          <ArrowLeft className="h-3.5 w-3.5" />
          Back to home
        </Link>
      </section>

      {/* Right: sign in */}
      <section className="flex min-h-screen items-center justify-center px-5 py-12 lg:min-h-0">
        <div className="w-full max-w-md space-y-8">
          <div className="lg:hidden">
            <Link to="/">
              <Wordmark tone="dark" />
            </Link>
          </div>

          <div>
            <p className="ledger-index">Sign in</p>
            <h2 className="mt-1.5 font-display text-3xl font-semibold text-ink">
              Pick an account
            </h2>
            <p className="mt-2 text-sm text-body-muted">
              Tap a demo role to fill the form, or use your own credentials.
            </p>
          </div>

          {/* Account picker: three tall cards, not a dropdown. Choosing a role
              is the first thing anyone does here, so it gets the space. */}
          <div className="grid gap-2">
            {demoAccounts.map((account) => {
              const active = picked === account.email;

              return (
                <button
                  key={account.email}
                  type="button"
                  onClick={() => useDemo(account.email)}
                  className={cx(
                    'group flex items-center justify-between gap-3 rounded-card border px-4 py-3 text-left transition-all',
                    active
                      ? 'border-ink bg-acid shadow-lift'
                      : 'border-rule bg-white hover:border-ink/40 hover:shadow-lift',
                  )}
                >
                  <span className="min-w-0">
                    <span className="block text-sm font-medium text-ink">
                      {account.role}
                    </span>
                    <span className="block truncate text-xs text-body-muted">
                      {account.blurb}
                    </span>
                  </span>

                  <ArrowRight
                    className={cx(
                      'h-4 w-4 shrink-0 transition-transform',
                      active
                        ? 'text-ink'
                        : 'text-body-faint group-hover:translate-x-0.5 group-hover:text-ink',
                    )}
                  />
                </button>
              );
            })}
          </div>

          <form onSubmit={handleSubmit} className="space-y-4">
            {error && <ErrorNote message={error} />}

            <Field label="Email">
              <Input
                type="email"
                value={email}
                autoComplete="username"
                required
                onChange={(event) => setEmail(event.target.value)}
                placeholder="you@edusphere.dev"
              />
            </Field>

            <Field label="Password">
              <Input
                type="password"
                value={password}
                autoComplete="current-password"
                required
                onChange={(event) => setPassword(event.target.value)}
                placeholder="••••••••"
              />
            </Field>

            <Button type="submit" loading={busy} className="w-full">
              Sign in
              <ArrowRight className="h-4 w-4" />
            </Button>
          </form>

          <p className="border-t border-rule pt-4 font-mono text-xs text-body-faint">
            demo password · {DEMO_PASSWORD}
          </p>
        </div>
      </section>
    </div>
  );
}
