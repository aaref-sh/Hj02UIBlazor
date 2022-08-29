/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [  "**/*.razor", "**/*.cshtml", "**/*.html" ],
  theme: {
      extend: {
          colors: {
              'blue-1000': '#092060'
          }
      },
  },
  plugins: [],
}
