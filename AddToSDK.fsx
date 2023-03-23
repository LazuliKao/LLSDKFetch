open System.Diagnostics
open System.IO
let currentPath=System.Environment.CurrentDirectory
let paths=[
    @"A:\Documents\GitHub\PixelFaramitaLuminousPolymerization\src\libs\LLSDK"
    @"A:\Documents\GitHub\LiteLoader.NET\SDK"
    @"A:\Documents\GitHub\LiteLoaderBDS-dotnet\SDK"
    @"A:\Documents\GitHub\PFEssentials\Libs\LLSDK"
    @"A:\Documents\GitHub\BETweaker\SDK"
    @"A:\Documents\GitHub\BDSpyrunner\SDK"
]
for path in paths do 
    for dir in Directory.GetDirectories(Path.Combine(currentPath,"SDK")) do
        let dirname=Path.GetFileName(dir)
        let targetDir=Path.Combine(path,dirname)
        if targetDir|>Directory.Exists then (targetDir,true)|>Directory.Delete
        if targetDir|>Path.GetDirectoryName|>Directory.Exists|>not &&
            targetDir|>Path.GetDirectoryName|>Path.GetDirectoryName|>Directory.Exists then targetDir|>Path.GetDirectoryName|>Directory.CreateDirectory|>ignore
        Process.Start("junction", [|
            "-s"
            targetDir
            dir
        |] )|>ignore