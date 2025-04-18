import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import fs from 'fs';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    strictPort: true,
    https: {
      key: fs.readFileSync('certs/localhost-key.pem'),
      cert: fs.readFileSync('certs/localhost.pem'),
    }
  }
})
