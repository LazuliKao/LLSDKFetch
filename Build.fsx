//一键生成BDSLib
open System.IO
open System.Diagnostics
let currentPath=System.Environment.CurrentDirectory
let BDSPath= @"A:\Documents\GitHub\BDS\Latest"
let wkd=Path.Combine(currentPath,"SDK","tools")
let outDir=Path.Combine(currentPath,"BDSLib")
printfn "生成BDS lib"
if Directory.Exists(outDir)|>not then Directory.CreateDirectory(outDir)|>ignore
Process.Start(ProcessStartInfo(Path.Combine(currentPath,"SDK","tools","LibraryBuilder.exe"),$"-o {outDir} {BDSPath}",WorkingDirectory=wkd))
    .WaitForExit()
for file in Directory.GetFiles(outDir) do 
    let filename=Path.GetFileName(file)
    let invokePath(tag:string)=
        let target=Path.Combine(currentPath,tag,"lib",filename)
        File.Copy(file,target,true)
        printfn "复制到%s" target
    invokePath "Release"
    invokePath "Develop"
if Directory.Exists(outDir) then Directory.Delete(outDir,true)