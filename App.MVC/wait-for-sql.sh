#!/bin/bash
set -e

host="$1"
shift
cmd="$@"

# انتظر حتى SQL Server يكون جاهز
until /opt/mssql-tools/bin/sqlcmd -S "$host" -U "sa" -P "Abc@123456" -Q "SELECT 1" > /dev/null 2>&1
do
  echo "Waiting for SQL Server at $host..."
  sleep 2
done

echo "SQL Server is up - executing command"
exec $cmd
