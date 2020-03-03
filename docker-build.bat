rem run it in the solution folder

docker build -f WebApi\Dockerfile . -t localhost:5000/demo-web-api:first
docker push localhost:5000/demo-web-api:first

docker build -f IdentityServerAspNetIdentity\Dockerfile . -t localhost:5000/demo-identity-server:first
docker push localhost:5000/demo-identity-server:first
