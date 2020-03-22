Demo solution to demonstrate how to develop .Net Core applications and use them in Kubernetes cluster.
In this branch IdentityServer4 replaced with Keycloak, see https://www.keycloak.org/

1. install docker-desktop https://www.docker.com/products/docker-desktop or https://docs.docker.com/engine/install/ on Linux
2. install helm 3 https://helm.sh/docs/intro/install/
3. run init.bat / init.sh script with admin privileges
4. open in browser http://api.demo.ebt.com/swagger
5. open in browser http://client.demo.ebt.com
6. open in browser http://mobile.demo.ebt.com

Use demo users: alice, bob. Password for both: Pass123$

Solution content:

	API: WebApi + Persistence
	Angular client: Client
	Mobile site (blazor server): Mobile

To uninstall:
	run clean.bat / clean.sh script
	uninstall helm
	uninstall docker-desktop

Init script does steps:
1. runs scripts from Init folder
2. builds images docker-build.bat
3. installs helm charts (run bat files from WebApi/charts, IdentityServerAspNetIdentity/charts, Client/charts, Mobile/charts)
4. adds hosts to /windows/system32/drivers/etc/hosts
	127.0.0.1 api.demo.ebt.com
	127.0.0.1 ids.demo.ebt.com
	127.0.0.1 client.demo.ebt.com
	127.0.0.1 mobile.demo.ebt.com
