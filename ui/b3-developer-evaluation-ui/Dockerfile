# Etapa 1: build da aplicação Angular
FROM node:20-alpine AS build

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build -- --output-path=dist

# Etapa 2: servir com Nginx
FROM nginx:alpine

# Remove a configuração padrão do Nginx
RUN rm /etc/nginx/conf.d/default.conf

# Copia a configuração customizada do Nginx (opcional)
# COPY nginx.conf /etc/nginx/conf.d

# Copia os arquivos buildados do Angular para o diretório público do Nginx
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 4200

CMD ["nginx", "-g", "daemon off;"]
