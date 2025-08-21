# Development Dockerfile for Frontend
FROM node:20-alpine AS build
WORKDIR /app

# Copy package.json and install dependencies
COPY frontend/angular-app/package*.json ./
RUN npm install

# Copy the rest of the application
COPY frontend/angular-app/ ./

# Expose port
EXPOSE 4200

# Start the application in development mode
CMD ["npm", "start", "--", "--host", "0.0.0.0"]
