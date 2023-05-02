//一键生成BDSLib
open System.IO
open System.Diagnostics
let currentPath=System.Environment.CurrentDirectory
let BDSPath= @"A:\Documents\GitHub\BDS\Latest"
let wkd=Path.Combine(currentPath,"Release","tools")
let outDir=Path.Combine(currentPath,"BDSLib")
System.Console.OutputEncoding=System.Text.Encoding.UTF8
printfn "生成BDS lib"
if Directory.Exists(outDir)|>not then Directory.CreateDirectory(outDir)|>ignore
//https://github.com/LiteLDev/PeEditor/blob/main/src/pe_editor/PeEditor.cpp#L383
Process.Start(ProcessStartInfo(Path.Combine(wkd,"PeEditor.exe"),$"-l -s -o {outDir} --pdb {BDSPath}\\bedrock_server.pdb",WorkingDirectory=wkd))
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