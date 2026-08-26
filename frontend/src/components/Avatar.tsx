import { useEffect, useState } from 'react';
import { cx } from '../lib/format';
import { useCustomAvatar } from '../lib/avatarStore';

export interface AvatarProps {
  email?: string | null;
  name?: string | null;
  id?: number | string | null;
  src?: string | null;
  alt?: string;
  size?: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | string;
  className?: string;
}

const sizeClasses: Record<string, string> = {
  xs: 'h-6 w-6 text-[10px]',
  sm: 'h-8 w-8 text-xs',
  md: 'h-9 w-9 text-xs',
  lg: 'h-10 w-10 text-sm',
  xl: 'h-12 w-12 text-base',
};

const tints = [
  'from-acid/40 to-moss/20',
  'from-flame/25 to-amberish/15',
  'from-ink/15 to-body-faint/10',
  'from-moss/25 to-acid/20',
];

export function getAvatarUrl(
  email?: string | null,
  name?: string | null,
  id?: number | string | null,
): string {
  const seed = (email || name || (id ? `user-${id}` : 'user')).trim().toLowerCase();
  return `https://i.pravatar.cc/150?u=${encodeURIComponent(seed)}`;
}

export function Avatar({
  email,
  name,
  id,
  src,
  alt,
  size = 'md',
  className,
}: AvatarProps) {
  const storedAvatar = useCustomAvatar(email, name, id);

  const [failed, setFailed] = useState(false);

  const avatarUrl = src || storedAvatar || getAvatarUrl(email, name, id);

  useEffect(() => {
    setFailed(false);
  }, [avatarUrl]);

  const displayAlt = alt || name || email || 'User avatar';

  const initials = (name || email || '?')
    .split(/[\s@]+/)
    .filter(Boolean)
    .slice(0, 2)
    .map((part) => part[0])
    .join('')
    .toUpperCase();

  const tintIndex = (initials.charCodeAt(0) || 0) % tints.length;
  const tint = tints[tintIndex];

  const dimensionClass = sizeClasses[size] || size;

  if (failed) {
    return (
      <div
        role="img"
        aria-label={displayAlt}
        className={cx(
          dimensionClass,
          'relative shrink-0 overflow-hidden rounded-full font-mono font-semibold',
          'flex items-center justify-center bg-gradient-to-br',
          tint,
          className,
        )}
      >
        <span className="relative select-none text-ink/80">{initials || '?'}</span>
      </div>
    );
  }

  return (
    <div
      className={cx(
        dimensionClass,
        'relative shrink-0 overflow-hidden rounded-full bg-paper-warm border border-ink/10',
        'flex items-center justify-center font-mono font-semibold',
        className,
      )}
    >
      <img
        src={avatarUrl}
        alt={displayAlt}
        loading="lazy"
        onError={() => setFailed(true)}
        className="h-full w-full object-cover rounded-full"
      />
    </div>
  );
}
