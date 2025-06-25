# imagem oficial
FROM nginx:alpine

# remoção da página padrão
RUN rm -rf /usr/share/nginx/html/*

# copiar os arquivos para a pasta do nginx
COPY . /usr/share/nginx/html

# expor a porta
EXPOSE 80
