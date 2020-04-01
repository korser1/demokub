#!/bin/sh

helm upgrade --install ids --reset-values --namespace default ./demo-identity-server -f ./demo-identity-server/values.yaml
