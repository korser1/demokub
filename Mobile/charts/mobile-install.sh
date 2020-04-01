#!/bin/sh

helm upgrade --install mobile --reset-values --namespace default ./demo-mobile-site -f ./demo-mobile-site/values.yaml
