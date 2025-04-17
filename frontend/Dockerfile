# Use node:20 as base image for building
FROM node:20 AS build
WORKDIR /app

# Copy package.json and package-lock.json first for caching dependencies
COPY package.json package-lock.json ./

# Install dependencies in a clean way
RUN npm ci

# Copy the rest of the app source code
COPY . .

# Build the project
RUN npm run build

# Use Nginx for serving the built app
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Copy built files from the previous stage
COPY --from=build /app/dist .

# Expose the port Nginx will serve on
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
