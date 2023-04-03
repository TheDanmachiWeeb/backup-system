/** @type {import('tailwindcss').Config} */
module.exports = {
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
  plugins: [],
}
