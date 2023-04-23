/** @type {import('tailwindcss').Config} */
module.exports = {
  variants: {
    display: [
      "children",
      "default",
      "children-first",
      "children-last",
      "children-odd",
      "children-even",
      "children-not-first",
      "children-not-last",
      "children-hover",
      "hover",
      "children-focus",
      "focus",
      "children-focus-within",
      "focus-within",
      "children-active",
      "active",
      "children-visited",
      "visited",
      "children-disabled",
      "disabled",
      "responsive",
    ],
  },
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      gradientColorStops: (theme) => ({
        pinky: "linear-gradient(156deg, #E9DEFA 17.26%, #FBFCDB 100%)",
      }),
      colors: {
        blacky: "#0F1322",
        sandy: "#D3B6A2",
        cyany: "#63DBE2",
      },
      backgroundImage: (theme) => ({
        pinky: theme("gradientColorStops.pinky"),
      }),
    },
  },
  plugins: [require("tailwindcss-children")],
};
