//Ò»¼üÍ¬²½LLSDK
#r "nuget: LibGit2Sharp, 0.26.2"
open System.Threading.Tasks
open System.Diagnostics
open LibGit2Sharp.Handlers
open LibGit2Sharp
open System.IO
let currentPath=System.Environment.CurrentDirectory
//let copyable=["Tools","Lib","Header"]
let download(branch:string)(target:string)=
    let tempPath=Path.Combine(currentPath,"temp")
    if tempPath|>Directory.Exists then 
        try
            (tempPath,true)|> Directory.Delete 
        with _->()
    let betaPath=Path.Combine(tempPath,Path.GetRandomFileName().[..7])
    printfn "%s : %s" branch betaPath   
    let callback=CheckoutProgressHandler(fun a b c->
        System.Console.CursorLeft<-System.Console.BufferWidth-1
        for i=0 to System.Console.BufferWidth do printf "\b"
        printf "%d / %d\t%s\t%d%%" b c a (b/c)
    )
    let targetGit="https://github.com/LiteLDev/SDK-cpp.git"
    //https://gitclone.com/github.com/LiteLDev/LiteLoaderSDK.git
    //let targetGit="https://gitclone.com/github.com/LiteLDev/LiteLoaderSDK.git"
    //let targetGit="https://ghproxy.com/https://github.com/LiteLDev/LiteLoaderSDK.git"
    //let targetGit="https://hub.njuu.cf/LiteLDev/SDK-cpp.git"
    let res=LibGit2Sharp.Repository.Clone(targetGit,betaPath,CloneOptions(BranchName=branch,OnCheckoutProgress=callback))
    printfn "\n%s" res
    let tp=Path.Combine(currentPath,target)
    printfn "Target : %s" tp
    if Directory.Exists(tp)|>not then Directory.CreateDirectory(tp)|>ignore
    for dir in Directory.GetDirectories(betaPath) do
        if dir.EndsWith(".git")|>not then
            let name=Path.GetFileName(dir)
            let target=Path.Combine(tp,name)
            if Directory.Exists(target) then Directory.Delete(target,true)
            Directory.Move(dir,target)
let downloadTask(branch:string)(target:string):Task=
    Task.Run(fun _->download branch target)
Task.WaitAll([|
    downloadTask "develop" "Develop"
    downloadTask "main" "Release"
    downloadTask "beta" "Beta"
|])
