#!/bin/sh

helm upgrade ing --install --reset-values --namespace kube-system stable/nginx-ingress -f ingress-values.yaml
