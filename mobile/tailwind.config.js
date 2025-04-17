/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./App.tsx', './src/**/*.{js,jsx,ts,tsx}'],
  presets: [require('nativewind/preset')],
  theme: {
    extend: {
      fontFamily: {
        sans: ['quicksand-medium'],
        'quicksand-medium': ['quicksand-medium'],
        'quicksand-semibold': ['quicksand-semibold'],
        'quicksand-bold': ['quicksand-bold'],
      },
      colors: {
        primary: '#3F6189',
        light: '#D5E9F9',
        lighter: '#EBF4FB',
        background: '#fbfbfb',
      },
    },
  },
  plugins: [],
};
