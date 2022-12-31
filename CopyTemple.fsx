open System.Diagnostics
open System.IO
let currentPath=System.Environment.CurrentDirectory
printfn "%A" fsi.CommandLineArgs
if Directory.Exists("SDK") then Directory.Delete("SDK",true)
//Process.Start("junction",
//[|
//"-s"
//Path.Combine(currentPath,"SDK")
//Path.Combine(currentPath,fsi.CommandLineArgs.[1])
//|]
//)
let rec copydir(source:string)(target:string)= 
    if not(Directory.Exists(target)) then Directory.CreateDirectory(target)|>ignore
    for file in Directory.GetFiles(source) do
        let copyTo=Path.Combine(target,Path.GetFileName(file))
        File.Copy(file,copyTo,true)
        if copyTo.EndsWith(@"SDK\include\llapi\Global.h") then
            let lines=File.ReadAllText(copyTo)
            let rep=lines.Replace("#include <entt/entt.hpp>","""
#pragma unmanaged
#include <entt/entt.hpp>
#pragma managed
"""             .Trim())
            File.WriteAllText(copyTo,rep)   
    for dir in Directory.GetDirectories(source) do
        copydir dir (Path.Combine(target,Path.GetFileName(dir)))
copydir(Path.Combine(currentPath,fsi.CommandLineArgs.[1]))(Path.Combine(currentPath,"SDK"))