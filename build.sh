#!/usr/bin/env bash
set -e
set -o pipefail
set -x

scriptcs ./baufile.csx -- "$@"
