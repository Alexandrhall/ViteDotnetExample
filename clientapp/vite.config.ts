import { defineConfig, type Plugin } from "vite";
import react from "@vitejs/plugin-react";

// UseReactDevelopmentServer waits for this exact line on stdout before it
// proxies any request, so print it first when Vite is actually listening
const dotnetReadySignal = (): Plugin => ({
  name: "dotnet-ready-signal",
  configureServer(server) {
    server.httpServer?.once("listening", () => {
      server.config.logger.info("Starting the development server");
    });
  },
});

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), dotnetReadySignal()],
  server: {
    port: 5173, // Vite frontend port
    strictPort: true, // Fail instead of switching port, the .NET proxy expects 5173
  },
});
