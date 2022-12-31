### LLSDKFetch
此F#脚本用于拾取[LiteLDev/SDK-cpp](https://github.com/LiteLDev/SDK-cpp)并构建lib
需.net 5+ sdk环境


#### UpdateAndRebuild.bat
从LL仓库拉取最新SDK并构建lib（通常运行这个，需要更新就SDK运行）

#### CopyBeta.bat CopyRelease.bat CopyDev.cmd
切换到某个分支

#### AddToSDK.cmd
这个要自行打开配置具体的路径，用于SDK路径的添加
且需要junction.exe，用于软链接

