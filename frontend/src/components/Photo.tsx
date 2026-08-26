import { useState } from 'react';

import { cx } from '../lib/format';

interface Props {
  /** Path under /public, e.g. "/images/teacher-1.jpg". */
  src: string;
  alt: string;
  /** Shown inside the placeholder while the real photo is missing. */
  label?: string;
  className?: string;
  /** Tailwind aspect utility, e.g. "aspect-[4/5]". */
  ratio?: string;
  rounded?: string;
}

const tints = [
  'from-acid/40 to-moss/20',
  'from-flame/25 to-amberish/15',
  'from-ink/15 to-body-faint/10',
  'from-moss/25 to-acid/20',
];

/**
 * An image that degrades into a designed panel instead of a broken icon.
 *
 * The repository ships without photographs on purpose (see
 * public/images/README.md), so every layout has to look finished with or
 * without them.
 */
export function Photo({
  src,
  alt,
  label,
  className,
  ratio = 'aspect-[4/5]',
  rounded = 'rounded-card',
}: Props) {
  const [failed, setFailed] = useState(false);

  const initials = (label ?? alt)
    .split(' ')
    .filter(Boolean)
    .slice(0, 2)
    .map((word) => word[0])
    .join('')
    .toUpperCase();

  // Stable tint per subject so the same person keeps the same panel colour.
  const tint = tints[(initials.charCodeAt(0) || 0) % tints.length];

  if (failed) {
    return (
      <div
        role="img"
        aria-label={alt}
        className={cx(
          ratio,
          rounded,
          'relative flex items-center justify-center overflow-hidden',
          'bg-gradient-to-br',
          tint,
          className,
        )}
      >
        <div
          aria-hidden
          className="absolute inset-0 opacity-[0.18]"
          style={{
            backgroundImage:
              'repeating-linear-gradient(45deg, transparent 0 8px, rgba(14,17,22,.5) 8px 9px)',
          }}
        />
        <span className="relative font-display text-3xl font-semibold text-ink/70">
          {initials || '· ·'}
        </span>
      </div>
    );
  }

  return (
    <img
      src={src}
      alt={alt}
      loading="lazy"
      onError={() => setFailed(true)}
      className={cx(ratio, rounded, 'w-full object-cover', className)}
    />
  );
}
