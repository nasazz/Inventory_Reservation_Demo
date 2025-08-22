# Development Dockerfile for Angular frontend
FROM node:20-alpine

WORKDIR /app

# Copy package.json + lock first for caching
COPY angular-app/package*.json ./

RUN npm install

# Copy rest of Angular app
COPY angular-app/ ./

# Expose dev server port
EXPOSE 4200

CMD ["npx", "ng", "serve", "--host", "0.0.0.0", "--port", "4200", "--disable-host-check"]


