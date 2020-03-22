#!/bin/sh

kubectl delete secret keycloak-db
kubectl create secret generic keycloak-db --from-file=db-password --from-file=db-user

kubectl delete secret keycloak-admin
kubectl create secret generic keycloak-admin --from-file=admin-password

kubectl delete secret realm-secret
kubectl create secret generic realm-secret --from-file=realm.json

helm upgrade demo-mysql --install --reset-values --namespace default stable/mysql -f mysql-values.yaml

helm upgrade demo-keycloak --install --reset-values --namespace default codecentric/keycloak -f keycloak-values.yaml
