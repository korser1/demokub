rem cleaning script
rem admin privileges required!

helm uninstall client
helm uninstall ids
helm uninstall mobile
helm uninstall api

rem remove from hosts
Set SiteList="client.demo.ebt.com" "ids.demo.ebt.com" "api.demo.ebt.com" "mobile.demo.ebt.com"
For %%A In (%Sitelist%) Do Call :Sub %%A
Exit/B
:Sub
@Powershell "(Get-Content """$($env:windir)\System32\drivers\etc\hosts""") | where {$_ -notmatch '^\s*127.0.0.1\s*%~1'} | Out-File """$($env:windir)\System32\drivers\etc\hosts""" -enc ASCII -Force"
