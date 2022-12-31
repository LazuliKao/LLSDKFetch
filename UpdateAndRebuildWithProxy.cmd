set http_proxy=http://127.0.0.1:2333 & set https_proxy=http://127.0.0.1:2333
@dotnet fsi Sync.fsx
@dotnet fsi Build.fsx
@pause
