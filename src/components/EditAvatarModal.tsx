import { useState, type ChangeEvent, type FormEvent } from 'react';
import { Camera, RotateCcw, Upload } from 'lucide-react';
import { Modal, Button, Field, ErrorNote } from './ui';
import { Avatar } from './Avatar';
import { getCustomAvatar, removeCustomAvatar, setCustomAvatar } from '../lib/avatarStore';

interface EditAvatarModalProps {
  open: boolean;
  onClose: () => void;
  email?: string | null;
  name?: string | null;
  id?: number | string | null;
}

export function EditAvatarModal({
  open,
  onClose,
  email,
  name,
  id,
}: EditAvatarModalProps) {
  const userIdentities = { email, name, id };
  const currentAvatar = getCustomAvatar(email, name, id);

  const [previewUrl, setPreviewUrl] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  function handleFileChange(event: ChangeEvent<HTMLInputElement>) {
    setError(null);
    const file = event.target.files?.[0];

    if (!file) return;

    if (!file.type.startsWith('image/')) {
      setError('Please select a valid image file (PNG, JPG, WebP, etc.).');
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      setError('Image size should be under 5 MB.');
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      if (typeof reader.result === 'string') {
        setPreviewUrl(reader.result);
      }
    };
    reader.onerror = () => {
      setError('Failed to read file.');
    };
    reader.readAsDataURL(file);
  }

  function handleSave(event: FormEvent) {
    event.preventDefault();
    setError(null);

    if (!previewUrl) {
      setError('Please choose an image file to upload.');
      return;
    }

    setCustomAvatar(userIdentities, previewUrl);
    setPreviewUrl(null);
    onClose();
  }

  function handleReset() {
    removeCustomAvatar(userIdentities);
    setPreviewUrl(null);
    onClose();
  }

  const activeDisplaySrc = previewUrl || undefined;

  return (
    <Modal open={open} onClose={onClose} title="Update Profile Picture">
      <form onSubmit={handleSave} className="space-y-5">
        {error && <ErrorNote message={error} />}

        {/* Live Preview */}
        <div className="flex flex-col items-center justify-center rounded-xl bg-paper-warm p-4 border border-rule">
          <div className="relative">
            <Avatar
              email={email}
              name={name}
              id={id}
              src={activeDisplaySrc}
              size="xl"
              className="ring-4 ring-acid/30"
            />
            <div className="absolute -bottom-1 -right-1 flex h-6 w-6 items-center justify-center rounded-full bg-ink text-acid">
              <Camera className="h-3.5 w-3.5" />
            </div>
          </div>
          <p className="mt-2 font-display text-sm font-semibold text-ink">{name || 'User'}</p>
          <p className="text-xs text-body-muted">{email || 'Profile Avatar'}</p>
        </div>

        {/* Upload Photo Option Only */}
        <Field label="Upload Photo" hint="Max size 5 MB. PNG, JPG, WebP.">
          <div className="flex items-center gap-3">
            <label className="flex flex-1 cursor-pointer items-center justify-center gap-2 rounded-lg border border-dashed border-ink/20 bg-white px-4 py-3 text-sm text-body-muted hover:border-ink hover:text-ink transition-colors">
              <Upload className="h-4 w-4" />
              <span>{previewUrl ? 'Change Selected File...' : 'Choose Image File...'}</span>
              <input
                type="file"
                accept="image/*"
                onChange={handleFileChange}
                className="hidden"
              />
            </label>
          </div>
        </Field>

        {/* Actions */}
        <div className="flex flex-wrap items-center justify-between gap-2 border-t border-rule pt-4">
          {currentAvatar ? (
            <Button
              type="button"
              variant="ghost"
              onClick={handleReset}
              className="text-xs text-flame hover:text-flame"
            >
              <RotateCcw className="h-3.5 w-3.5" />
              Reset to Default
            </Button>
          ) : (
            <div />
          )}

          <div className="flex gap-2">
            <Button type="button" variant="secondary" onClick={onClose}>
              Cancel
            </Button>
            <Button type="submit" disabled={!previewUrl}>
              Save Picture
            </Button>
          </div>
        </div>
      </form>
    </Modal>
  );
}
