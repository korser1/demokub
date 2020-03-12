Demo solution to demonstrate how to develop .Net Core applications and use them in Kubernetes cluster.


1. install docker-desktop https://www.docker.com/products/docker-desktop
2. install helm 3 https://helm.sh/
3. run scripts from Init folder
4. build images docker-build.bat
5. install helm charts (run bat files from WebApi/charts, IdentityServerAspNetIdentity/charts, Client/charts)
6. add hosts to /windows/system32/drivers/etc/hosts
	127.0.0.1 api.demo.ebt.com
	127.0.0.1 ids.demo.ebt.com
	127.0.0.1 client.demo.ebt.com
7. open in browser http://api.demo.ebt.com/swagger
8. open in browser http://client.demo.ebt.com

Solution content:

	API: WebApi + Persistence
	Identity Server: IdentityServerAspNetIdentity
	Angular client: Client
