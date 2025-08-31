import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // forwards /api/* to your backend
      "/api": {
        target: "https://localhost:7130",
        changeOrigin: true,
        secure: false, // allow dev HTTPS without a trusted cert
      },
    },
  },
});
