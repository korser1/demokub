rem run it in the solution folder

docker build -f WebApi\Dockerfile . -t localhost:5000/demo-web-api:first
docker push localhost:5000/demo-web-api:first

docker build -f IdentityServerAspNetIdentity\Dockerfile . -t localhost:5000/demo-identity-server:first
docker push localhost:5000/demo-identity-server:first

docker build -f Mobile\Dockerfile . -t localhost:5000/demo-mobile-site:first
docker push localhost:5000/demo-mobile-site:first

cd Client
docker build -f Dockerfile . -t localhost:5000/demo-angular-client:first
docker push localhost:5000/demo-angular-client:first
cd ..
