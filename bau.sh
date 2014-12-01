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
    rm -f ./ScriptCs.0.10.2.nupkg
    wget "http://chocolateypackages.s3.amazonaws.com/ScriptCs.0.10.2.nupkg"
    unzip ./ScriptCs.0.10.2.nupkg -d scriptcs
fi

mono ./scriptcs/tools/scriptcs/scriptcs.exe -install
if [ -d ./packages/Bau.XUnit.0.1.0-beta07 ]
  then
    mv ./packages/Bau.XUnit.0.1.0-beta07/Bau.XUnit.0.1.0-beta07.nupkg ./packages/Bau.XUnit.0.1.0-beta07/Bau.Xunit.0.1.0-beta07.nupkg
    mv ./packages/Bau.XUnit.0.1.0-beta07 ./packages/Bau.Xunit.0.1.0-beta07
fi

mono ./packages/NuGet.CommandLine.2.8.3/tools/NuGet.exe restore src/Bau.sln
  
# script
mono ./scriptcs/tools/scriptcs/scriptcs.exe ./mono.csx -- $@
