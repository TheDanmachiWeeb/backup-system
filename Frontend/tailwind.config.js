/** @type {import('tailwindcss').Config} */
module.exports = {
  variants: {
    display: ['children', 'default', 'children-first', 'children-last', 'children-odd', 'children-even', 'children-not-first', 'children-not-last', 'children-hover', 'hover', 'children-focus', 'focus', 'children-focus-within', 'focus-within', 'children-active', 'active', 'children-visited', 'visited', 'children-disabled', 'disabled', 'responsive'],
  },
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: '#414C59', // Dark blue
        secondary: '#E0E6ED', // Light gray
        tertiary: '#F4F5F7', // Off-white
        success: '#4CAF50', // Green
        warning: '#FFC107', // Orange
        danger: '#FF5252', // Red
      },
    },
  },
  plugins: [ require('tailwindcss-children'),],
}
