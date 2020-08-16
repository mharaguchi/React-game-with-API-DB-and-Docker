FROM mcr.microsoft.com/mssql/server:2017-CU17-ubuntu

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

COPY . /usr/src/app

ENV ACCEPT_EULA Y
ENV sa_password Password!
ENV MSSQL_PID Express

CMD /bin/bash ./entrypoint.sh
# CMD /opt/mssql-tools/bin/sqlcmd -i /usr/src/app/init.sql && .\start -sa_password $env:sa_password \
# -ACCEPT_EULA $env:ACCEPT_EULA -attach_dbs \"$env:attach_dbs\" -Verbose
