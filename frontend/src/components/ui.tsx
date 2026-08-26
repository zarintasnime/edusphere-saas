import type {
  ButtonHTMLAttributes,
  InputHTMLAttributes,
  ReactNode,
  SelectHTMLAttributes,
  TextareaHTMLAttributes,
} from 'react';
import { Loader2, X } from 'lucide-react';

import { cx } from '../lib/format';

/* -------------------------------------------------------------------------- */
/* Buttons                                                                     */
/* -------------------------------------------------------------------------- */

type ButtonVariant = 'primary' | 'accent' | 'secondary' | 'ghost' | 'danger';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant;
  loading?: boolean;
  size?: 'sm' | 'md';
}

/**
 * Buttons are square-shouldered with a hard 1px edge and a small press shift,
 * so they read as physical controls rather than floating pills.
 */
const buttonStyles: Record<ButtonVariant, string> = {
  primary:
    'bg-ink text-paper border-ink hover:bg-ink-soft active:translate-y-px ' +
    'disabled:bg-body-faint disabled:border-body-faint',
  accent:
    'bg-acid text-ink border-ink hover:bg-acid-deep active:translate-y-px ' +
    'disabled:bg-acid/40',
  secondary:
    'bg-transparent text-ink border-ink/25 hover:border-ink hover:bg-paper-warm ' +
    'active:translate-y-px disabled:text-body-faint disabled:border-rule',
  ghost:
    'bg-transparent text-body-muted border-transparent hover:bg-paper-warm hover:text-ink',
  danger:
    'bg-flame text-white border-flame hover:brightness-95 active:translate-y-px',
};

export function Button({
  variant = 'primary',
  loading = false,
  size = 'md',
  className,
  children,
  disabled,
  ...rest
}: ButtonProps) {
  return (
    <button
      {...rest}
      disabled={disabled || loading}
      className={cx(
        'inline-flex items-center justify-center gap-2 rounded-lg border',
        'font-medium transition-all duration-150 disabled:cursor-not-allowed',
        size === 'sm' ? 'px-2.5 py-1.5 text-xs' : 'px-4 py-2 text-sm',
        buttonStyles[variant],
        className,
      )}
    >
      {loading && <Loader2 className="h-3.5 w-3.5 animate-spin" aria-hidden />}
      {children}
    </button>
  );
}

/* -------------------------------------------------------------------------- */
/* Form fields                                                                 */
/* -------------------------------------------------------------------------- */

const controlClasses =
  'w-full rounded-lg border border-rule bg-white px-3 py-2 text-sm text-ink ' +
  'placeholder:text-body-faint transition-colors focus:border-ink ' +
  'focus:outline-none focus:ring-4 focus:ring-acid/40 disabled:bg-paper-warm';

export function Field({
  label,
  hint,
  children,
}: {
  label: string;
  hint?: string;
  children: ReactNode;
}) {
  return (
    <label className="block space-y-1.5">
      <span className="font-mono text-[11px] uppercase tracking-[0.14em] text-body-muted">
        {label}
      </span>
      {children}
      {hint && <span className="block text-xs text-body-faint">{hint}</span>}
    </label>
  );
}

export const Input = (props: InputHTMLAttributes<HTMLInputElement>) => (
  <input {...props} className={cx(controlClasses, props.className)} />
);

export const Textarea = (props: TextareaHTMLAttributes<HTMLTextAreaElement>) => (
  <textarea {...props} className={cx(controlClasses, 'min-h-24', props.className)} />
);

export const Select = (props: SelectHTMLAttributes<HTMLSelectElement>) => (
  <select {...props} className={cx(controlClasses, 'pr-8', props.className)} />
);

/* -------------------------------------------------------------------------- */
/* Surfaces                                                                    */
/* -------------------------------------------------------------------------- */

export function Card({
  children,
  className,
}: {
  children: ReactNode;
  className?: string;
}) {
  return (
    <div
      className={cx(
        'rounded-card border border-rule bg-white shadow-lift',
        className,
      )}
    >
      {children}
    </div>
  );
}

export function SectionHeading({
  index,
  title,
  action,
}: {
  index: string;
  title: string;
  action?: ReactNode;
}) {
  return (
    <div className="mb-3 flex items-end justify-between gap-3">
      <div>
        <p className="ledger-index">{index}</p>
        <h2 className="mt-0.5 text-lg font-semibold text-ink">{title}</h2>
      </div>
      {action}
    </div>
  );
}

export function PageHeader({
  title,
  subtitle,
  action,
}: {
  title: string;
  subtitle?: string;
  action?: ReactNode;
}) {
  return (
    <div className="mb-7 flex flex-wrap items-end justify-between gap-4 border-b border-rule pb-5">
      <div>
        <h1 className="text-2xl font-semibold text-ink sm:text-3xl">{title}</h1>
        {subtitle && <p className="mt-1.5 text-sm text-body-muted">{subtitle}</p>}
      </div>
      {action}
    </div>
  );
}

/** Big number in a ledger cell. Mono, oversized, unapologetic. */
export function StatCard({
  label,
  value,
  caption,
  tone = 'plain',
}: {
  label: string;
  value: string | number;
  caption?: string;
  tone?: 'plain' | 'accent' | 'dark';
}) {
  return (
    <div
      className={cx(
        'rounded-card border p-4',
        tone === 'dark' && 'border-ink bg-ink text-paper',
        tone === 'accent' && 'border-ink bg-acid text-ink',
        tone === 'plain' && 'border-rule bg-white',
      )}
    >
      <p
        className={cx(
          'font-mono text-[11px] uppercase tracking-[0.14em]',
          tone === 'dark' ? 'text-paper/60' : 'text-body-muted',
        )}
      >
        {label}
      </p>
      <p className="mt-3 font-display text-4xl font-semibold leading-none">
        {value}
      </p>
      {caption && (
        <p
          className={cx(
            'mt-2 text-xs',
            tone === 'dark' ? 'text-paper/50' : 'text-body-faint',
          )}
        >
          {caption}
        </p>
      )}
    </div>
  );
}

/* -------------------------------------------------------------------------- */
/* Badges                                                                      */
/* -------------------------------------------------------------------------- */

type Tone = 'neutral' | 'ink' | 'accent' | 'open' | 'soon' | 'over';

const toneStyles: Record<Tone, string> = {
  neutral: 'bg-paper-warm text-body-muted border-rule',
  ink: 'bg-ink text-paper border-ink',
  accent: 'bg-acid text-ink border-ink/20',
  open: 'bg-moss-ghost text-moss border-moss/25',
  soon: 'bg-amberish-ghost text-amberish border-amberish/25',
  over: 'bg-flame-ghost text-flame border-flame/25',
};

export function Badge({
  tone = 'neutral',
  children,
}: {
  tone?: Tone;
  children: ReactNode;
}) {
  return (
    <span
      className={cx(
        'inline-flex items-center rounded-full border px-2.5 py-0.5',
        'font-mono text-[11px] font-medium tracking-tight',
        toneStyles[tone],
      )}
    >
      {children}
    </span>
  );
}

/** Identifiers read as data, not prose. */
export const Code = ({ children }: { children: ReactNode }) => (
  <span className="font-mono text-xs text-body-muted">{children}</span>
);

/* -------------------------------------------------------------------------- */
/* States                                                                      */
/* -------------------------------------------------------------------------- */

export function Spinner({ label }: { label?: string }) {
  return (
    <div className="flex items-center gap-2 px-4 py-10 text-sm text-body-muted">
      <Loader2 className="h-4 w-4 animate-spin" aria-hidden />
      {label ?? 'Loading'}
    </div>
  );
}

export function FullPageSpinner({ label }: { label?: string }) {
  return (
    <div className="flex min-h-screen items-center justify-center bg-paper">
      <Spinner label={label} />
    </div>
  );
}

export function ErrorNote({ message }: { message: string }) {
  return (
    <div className="rounded-lg border border-flame/25 bg-flame-ghost px-3 py-2 text-sm text-flame">
      {message}
    </div>
  );
}

export function EmptyState({
  title,
  description,
  action,
}: {
  title: string;
  description: string;
  action?: ReactNode;
}) {
  return (
    <div className="flex flex-col items-center gap-2 px-6 py-14 text-center">
      <span
        aria-hidden
        className="mb-1 h-8 w-8 rounded-full border border-dashed border-rule"
      />
      <p className="font-display text-lg font-semibold text-ink">{title}</p>
      <p className="max-w-sm text-sm text-body-muted">{description}</p>
      {action}
    </div>
  );
}

/* -------------------------------------------------------------------------- */
/* Modal                                                                       */
/* -------------------------------------------------------------------------- */

export function Modal({
  open,
  title,
  onClose,
  children,
}: {
  open: boolean;
  title: string;
  onClose: () => void;
  children: ReactNode;
}) {
  if (!open) return null;

  return (
    <div
      className="fixed inset-0 z-50 flex items-start justify-center overflow-y-auto bg-ink/50 p-4 backdrop-blur-[2px] sm:p-8"
      role="dialog"
      aria-modal="true"
      aria-label={title}
    >
      <div className="w-full max-w-xl animate-rise rounded-card border border-ink/10 bg-paper shadow-lift">
        <div className="flex items-center justify-between border-b border-rule px-5 py-4">
          <h2 className="font-display text-lg font-semibold text-ink">{title}</h2>
          <button
            type="button"
            onClick={onClose}
            aria-label="Close"
            className="rounded-md p-1.5 text-body-muted transition-colors hover:bg-paper-warm hover:text-ink"
          >
            <X className="h-4 w-4" />
          </button>
        </div>
        <div className="px-5 py-5">{children}</div>
      </div>
    </div>
  );
}

/* -------------------------------------------------------------------------- */
/* Table                                                                       */
/* -------------------------------------------------------------------------- */

export function Table({
  head,
  children,
}: {
  head: string[];
  children: ReactNode;
}) {
  return (
    <div className="overflow-x-auto">
      <table className="w-full min-w-[36rem] text-left text-sm">
        <thead>
          <tr className="border-b border-ink/15">
            {head.map((column) => (
              <th
                key={column}
                className="px-4 py-3 font-mono text-[11px] font-medium uppercase tracking-[0.14em] text-body-muted"
              >
                {column}
              </th>
            ))}
          </tr>
        </thead>
        <tbody className="divide-y divide-rule">{children}</tbody>
      </table>
    </div>
  );
}

export const Td = ({
  children,
  className,
}: {
  children: ReactNode;
  className?: string;
}) => <td className={cx('px-4 py-3.5 align-middle', className)}>{children}</td>;
