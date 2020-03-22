rem full install script
rem admin privileges required!

cd Init
call docker-repo.bat
call helm-init.bat
call ingress.bat
call keycloak.bat
cd ..

call docker-build.bat

cd Client/charts
call client-install.bat
cd ../../

cd Mobile/charts
call mobile-install.bat
cd ../../

cd WebApi/charts
call api-install.bat
cd ../../

set curdir=%cd%
cd %WINDIR%/system32/drivers/etc

findstr /C:"client.demo.ebt.com" hosts
if %ERRORLEVEL% == 1 ECHO 127.0.0.1 client.demo.ebt.com >> hosts

findstr /C:"ids.demo.ebt.com" hosts
if %ERRORLEVEL% == 1 ECHO 127.0.0.1 ids.demo.ebt.com >> hosts

findstr /C:"api.demo.ebt.com" hosts
if %ERRORLEVEL% == 1 ECHO 127.0.0.1 api.demo.ebt.com >> hosts

findstr /C:"mobile.demo.ebt.com" hosts
if %ERRORLEVEL% == 1 ECHO 127.0.0.1 mobile.demo.ebt.com >> hosts

cd %curdir%
