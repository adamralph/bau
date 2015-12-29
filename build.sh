#!/usr/bin/env bash
set -e
set -o pipefail
set -x

exec scriptcs ./baufile.csx -- "$@"
