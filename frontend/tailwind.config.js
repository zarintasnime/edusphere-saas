/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        // Ink-first palette. Near-black surfaces, paper-white content,
        // one loud accent. Deliberately not another blue admin theme.
        ink: {
          DEFAULT: '#0E1116',
          soft: '#161B23',
          line: '#232A35',
        },
        paper: {
          DEFAULT: '#FAFAF7',
          warm: '#F2F1EA',
        },
        body: {
          DEFAULT: '#14181F',
          muted: '#5A6472',
          faint: '#8C95A3',
        },
        rule: '#E4E3DB',
        acid: {
          DEFAULT: '#C8FF4D',
          deep: '#A8E020',
          ghost: '#F0FFCC',
        },
        flame: {
          DEFAULT: '#FF5C38',
          ghost: '#FFE9E3',
        },
        moss: {
          DEFAULT: '#0F7B4F',
          ghost: '#DFF5E9',
        },
        amberish: {
          DEFAULT: '#9A5B00',
          ghost: '#FFF0D4',
        },
      },
      fontFamily: {
        display: ['Fraunces', 'Georgia', 'serif'],
        sans: ['Inter', 'system-ui', 'sans-serif'],
        mono: ['"JetBrains Mono"', 'ui-monospace', 'monospace'],
      },
      borderRadius: {
        card: '14px',
      },
      boxShadow: {
        lift: '0 1px 2px rgba(14,17,22,.05), 0 12px 32px -20px rgba(14,17,22,.35)',
        stamp: 'inset 0 0 0 1px rgba(14,17,22,.12)',
      },
      keyframes: {
        marquee: {
          '0%': { transform: 'translateX(0)' },
          '100%': { transform: 'translateX(-50%)' },
        },
        rise: {
          '0%': { opacity: '0', transform: 'translateY(10px)' },
          '100%': { opacity: '1', transform: 'translateY(0)' },
        },
        pulseDot: {
          '0%,100%': { opacity: '1' },
          '50%': { opacity: '.35' },
        },
      },
      animation: {
        marquee: 'marquee 32s linear infinite',
        rise: 'rise .4s ease-out both',
        pulseDot: 'pulseDot 2s ease-in-out infinite',
      },
    },
  },
  plugins: [],
};
