#!/bin/sh

helm upgrade --install client --reset-values --namespace default ./demo-angular-client -f ./demo-angular-client/values.yaml
