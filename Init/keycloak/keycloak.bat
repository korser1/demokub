
kubectl delete secret keycloak-db
kubectl create secret generic keycloak-db --from-file=DB_PASSWORD --from-file=DB_USER

kubectl delete secret keycloak-admin
kubectl create secret generic keycloak-admin --from-file=KEYCLOAK_PASSWORD

kubectl delete secret realm-secret
kubectl create secret generic realm-secret --from-file=realm.json

kubectl delete secret keycloak-mysql
kubectl create secret generic keycloak-mysql --from-file=mysql-password --from-file=mysql-root-password

kubectl delete secret tls-keycloak
kubectl create secret generic tls-keycloak --from-file=./config/tls.crt --from-file=./config/tls.key

helm upgrade demo-mysql --install --reset-values --namespace default stable/mysql -f mysql-values.yaml

helm upgrade demo-keycloak --install --reset-values --namespace default codecentric/keycloak -f keycloak-values.yaml
