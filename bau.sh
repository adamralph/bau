#!/usr/bin/env bash
set -e
set -o pipefail
set -x

# before_install
# sudo bash -c "echo deb http://badgerports.org precise main >> /etc/apt/sources.list"
# sudo apt-key adv --keyserver keyserver.ubuntu.com --recv-keys 0E1FAD0C
# sudo apt-get update -qq
# sudo apt-get install -qq mono-devel
# mozroots --import --sync

# install
if [ ! -d ./scriptcs ]
  then
    rm -f ./scriptcs-0.10.0-alpha.140530.nupkg
    wget "https://github.com/bau-build/bau-blob/raw/master/scriptcs-0.10.0-alpha.140530.nupkg"
    unzip ./scriptcs-0.10.0-alpha.140530.nupkg -d scriptcs
fi

mono ./scriptcs/tools/scriptcs/scriptcs.exe -install
if [ -d ./packages/Bau.XUnit.0.1.0-beta06 ]
  then
    mv ./packages/Bau.XUnit.0.1.0-beta06/Bau.XUnit.0.1.0-beta06.nupkg ./packages/Bau.XUnit.0.1.0-beta06/Bau.Xunit.0.1.0-beta06.nupkg
    mv ./packages/Bau.XUnit.0.1.0-beta06 ./packages/Bau.Xunit.0.1.0-beta06
fi

mono ./packages/NuGet.CommandLine.2.8.2/tools/NuGet.exe restore src/Bau.sln
  
# script
mono ./scriptcs/tools/scriptcs/scriptcs.exe ./mono.csx -- $@
