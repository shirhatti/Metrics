#!/bin/sh

kubectl apply -f kubernetes/prometheus-deployment.yml
kubectl apply -f kubernetes/webapp-deployment.yml
