import { useEffect, useState } from 'react';

const STORAGE_PREFIX = 'campusflow_avatar_';
const AVATAR_CHANGE_EVENT = 'campusflow_avatar_changed';

export interface UserIdentities {
  email?: string | null;
  name?: string | null;
  id?: number | string | null;
}

export function getUserSeeds(
  email?: string | null,
  name?: string | null,
  id?: number | string | null,
): string[] {
  const seeds: string[] = [];
  if (email && email.trim()) seeds.push(email.trim().toLowerCase());
  if (name && name.trim()) seeds.push(name.trim().toLowerCase());
  if (id !== undefined && id !== null && String(id).trim()) seeds.push(`user-${id}`);
  return seeds;
}

export function getCustomAvatar(
  email?: string | null,
  name?: string | null,
  id?: number | string | null,
): string | null {
  const seeds = getUserSeeds(email, name, id);
  for (const seed of seeds) {
    try {
      const val = localStorage.getItem(`${STORAGE_PREFIX}${seed}`);
      if (val) return val;
    } catch {
      // ignore storage errors
    }
  }
  return null;
}

export function setCustomAvatar(
  identities: UserIdentities | string,
  avatarUrl: string,
): void {
  const seeds =
    typeof identities === 'string'
      ? [identities.trim().toLowerCase()]
      : getUserSeeds(identities.email, identities.name, identities.id);

  if (seeds.length === 0) return;

  try {
    for (const seed of seeds) {
      localStorage.setItem(`${STORAGE_PREFIX}${seed}`, avatarUrl);
      window.dispatchEvent(
        new CustomEvent(AVATAR_CHANGE_EVENT, { detail: { seed, avatarUrl } }),
      );
    }
  } catch (err) {
    console.error('Failed to save avatar to localStorage:', err);
  }
}

export function removeCustomAvatar(
  identities: UserIdentities | string,
): void {
  const seeds =
    typeof identities === 'string'
      ? [identities.trim().toLowerCase()]
      : getUserSeeds(identities.email, identities.name, identities.id);

  if (seeds.length === 0) return;

  try {
    for (const seed of seeds) {
      localStorage.removeItem(`${STORAGE_PREFIX}${seed}`);
      window.dispatchEvent(
        new CustomEvent(AVATAR_CHANGE_EVENT, { detail: { seed, avatarUrl: null } }),
      );
    }
  } catch (err) {
    console.error('Failed to remove avatar from localStorage:', err);
  }
}

export function useCustomAvatar(
  email?: string | null,
  name?: string | null,
  id?: number | string | null,
): string | null {
  const [avatar, setAvatar] = useState<string | null>(() =>
    getCustomAvatar(email, name, id),
  );

  useEffect(() => {
    setAvatar(getCustomAvatar(email, name, id));

    function handleAvatarChange(event: Event) {
      const customEvent = event as CustomEvent<{ seed: string; avatarUrl: string | null }>;
      const seeds = getUserSeeds(email, name, id);
      if (!customEvent.detail || seeds.includes(customEvent.detail.seed)) {
        setAvatar(getCustomAvatar(email, name, id));
      }
    }

    window.addEventListener(AVATAR_CHANGE_EVENT, handleAvatarChange);
    return () => {
      window.removeEventListener(AVATAR_CHANGE_EVENT, handleAvatarChange);
    };
  }, [email, name, id]);

  return avatar;
}
