#!/bin/bash

set -m

consul agent -node $NODE -dns-port 53 -data-dir /data -config-dir /etc/consul.d -join consul_server