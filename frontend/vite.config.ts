import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'node:path';

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: { '@': path.resolve(__dirname, './src') },
  },
  server: {
    // The API allows 5173 and 5174 by default (see Cors:AllowedOrigins).
    port: 5173,
    strictPort: false,
  },
});
