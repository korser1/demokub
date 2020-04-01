#!/bin/sh

helm upgrade --install api --reset-values --namespace default ./demo-web-api -f ./demo-web-api/values.yaml
