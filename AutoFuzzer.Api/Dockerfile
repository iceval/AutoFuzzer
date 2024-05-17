FROM ubuntu:bionic AS afl-env
WORKDIR /app

RUN apt-get update && apt-get install -y g++ make wget

COPY /4.05c.tar.gz .
RUN tar -xvf 4.05c.tar.gz

WORKDIR AFLplusplus-4.05c
RUN make

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global SharpFuzz.CommandLine

COPY FuzzerApp FuzzerApp
COPY main-script.sh FuzzerApp/main-script.sh
COPY run.sh FuzzerApp/run.sh
COPY rename.sh FuzzerApp/rename.sh

WORKDIR /app/FuzzerApp
RUN dotnet restore -r linux-x64

RUN dotnet publish -r linux-x64 --self-contained --no-restore -c release -o out

COPY --from=afl-env /app/AFLplusplus-4.05c ./afl/

COPY FuzzerApp/Main/Dictionaries ./dictionaries/
COPY FuzzerApp/Main/TestCases ./testcases/


VOLUME /app/FuzzerApp/findings/default
ENV AFL_SKIP_BIN_CHECK=true
CMD /bin/bash ./main-script.sh

