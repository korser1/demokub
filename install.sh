# full install script
# admin privileges required!

cd Init
./docker-repo.sh
./helm-init.sh
./ingress.sh
cd ..

./docker-build.sh

cd Client/charts
./client-install.sh
cd ../../

cd IdentityServerAspNetIdentity/charts
./ids-install.sh
cd ../../

cd Mobile/charts
./mobile-install.sh
cd ../../

cd WebApi/charts
./api-install.sh
cd ../../

if ! grep -Fq "client.demo.ebt.com" /etc/hosts; then
  echo "127.0.0.1 client.demo.ebt.com" >> /etc/hosts
fi

if ! grep -Fq "ids.demo.ebt.com" /etc/hosts; then
  echo "127.0.0.1 ids.demo.ebt.com" >> /etc/hosts
fi

if ! grep -Fq "api.demo.ebt.com" /etc/hosts; then
  echo "127.0.0.1 api.demo.ebt.com" >> /etc/hosts
fi

if ! grep -Fq "mobile.demo.ebt.com" /etc/hosts; then
  echo "127.0.0.1 mobile.demo.ebt.com" >> /etc/hosts
fi
