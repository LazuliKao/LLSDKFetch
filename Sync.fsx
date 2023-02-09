//һ��ͬ��LLSDK
#r "nuget: LibGit2Sharp, 0.26.2"
#r "nuget: ShellProgressBar, 5.2.0"
open System.Threading.Tasks
open System.Diagnostics
open LibGit2Sharp.Handlers
open LibGit2Sharp
open System.IO
let currentPath=System.Environment.CurrentDirectory
//let copyable=["Tools","Lib","Header"]

let download(branch:string)(target:string)=
    let progress=new ShellProgressBar.ProgressBar(100,target+"@git clone https://github.com/LiteLDev/SDK-cpp.git",
        ShellProgressBar.ProgressBarOptions(
            ProgressCharacter = '─',
            ProgressBarOnBottom = true,
            ShowEstimatedDuration=true,
            ForegroundColor = System.ConsoleColor.Yellow,
            ForegroundColorDone = System.ConsoleColor.DarkGreen,
            BackgroundColor = System.ConsoleColor.DarkGray,
            BackgroundCharacter = '\u2593',
            DisplayTimeInRealTime = false
        ))
    let tempPath=Path.Combine(currentPath,"temp")
    if tempPath|>Directory.Exists then
        try
            (tempPath,true)|> Directory.Delete
        with _->()
    let betaPath=Path.Combine(tempPath,Path.GetRandomFileName().[..7])
    printfn "%s : %s" branch betaPath
    let callback=CheckoutProgressHandler(fun a b c->
        //System.Console.CursorLeft<-System.Console.BufferWidth-1
        //for i=0 to System.Console.BufferWidth do printf "\b"
        //printf "%d / %d\t%s\t%d%%" b c a (b/c)
        //progress.Report(b/c);
        progress.Tick($"{b}/{c} {a}")
    )
    let callbackt=ProgressHandler(fun str->
        //System.Console.CursorLeft<-System.Console.BufferWidth-1
        //for i=0 to System.Console.BufferWidth do printf "\b"
        //printf "%d / %d\t%s\t%d%%" b c a (b/c)
        //progress.Report(b/c);
        progress.Tick(str)
        true
    )
    let callupdatetips=UpdateTipsHandler(fun str _ _->
        progress.Tick(str)
        true
    )
    //let targetGit="https://github.com/LiteLDev/SDK-cpp.git"
    //https://gitclone.com/github.com/LiteLDev/SDK-cpp.git
    //let targetGit="https://gitclone.com/github.com/LiteLDev/SDK-cpp.git"
    let targetGit="https://ghproxy.com/https://github.com/LiteLDev/SDK-cpp.git"
    //let targetGit="https://hub.njuu.cf/LiteLDev/SDK-cpp.git"
    let res=LibGit2Sharp.Repository.Clone(targetGit,betaPath,CloneOptions(BranchName=branch,OnProgress=callbackt,OnCheckoutProgress=callback,OnUpdateTips=callupdatetips))
    printfn "\n%s" res
    let tp=Path.Combine(currentPath,target)
    printfn "Target : %s" tp
    if Directory.Exists(tp)|>not then Directory.CreateDirectory(tp)|>ignore
    for dir in Directory.GetDirectories(betaPath) do
        if dir.EndsWith(".git")|>not then
            let name=Path.GetFileName(dir)
            let target=Path.Combine(tp,name)
            if Directory.Exists(target) then Directory.Delete(target,true) 
            if Path.GetFileName(dir).StartsWith "_git2" |> not then 
                Directory.Move(dir,target)
let downloadTask(branch:string)(target:string)=
    async{
        download branch target
    }
[
    downloadTask "develop" "Develop"
    downloadTask "main" "Release"
    //downloadTask "beta" "Beta"
]
|>Async.Parallel
|>Async.RunSynchronously