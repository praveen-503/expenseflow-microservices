# Stage 1: Build Angular application
FROM node:22-alpine AS build
WORKDIR /app
COPY src/Web/expenseflow-client/package*.json ./
RUN npm ci
COPY src/Web/expenseflow-client/ ./
RUN npm run build -- --configuration=production

# Stage 2: Serve using Nginx
FROM nginx:alpine
COPY --from=build /app/dist/expenseflow-client/browser /usr/share/nginx/html
COPY src/Web/expenseflow-client/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
