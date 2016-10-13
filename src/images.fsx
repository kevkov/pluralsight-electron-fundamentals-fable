#r "../node_modules/fable-core/Fable.Core.dll"
#load "../node_modules/fable-import-electron/Fable.Import.Electron.fs"

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser
open Fable.Import.Node
open Fable.Import.Electron
open Fable.Import.JS
open System.Collections.Generic

let imageCache = List<string>()

let logError (err:string) = printfn "%s" err

let createFilename (date: System.DateTime) =
    (sprintf "%s-%s.png" (date.ToLongDateString()) (date.ToLongTimeString())).Replace(":", "-")

let saveImage picturesPath (contents: string) (donefn: string -> unit) =
    let base64Data = contents.Replace("data:image/png;base64,", "")
    let date = Date.Create()
    let imgPath = path.join(picturesPath, createFilename date)

    let callback (err:NodeJS.ErrnoException) =
        match err with
        | null -> donefn(imgPath)
        | _ -> logError(err.ToString())
        
    printfn "writing"
    fs.writeFile(imgPath, base64Data, createObj ["encoding" ==> "base64"], System.Func<_,_>callback)

let getPicturesDir (app: App) =
    let dir = path.join(app.getPath(AppPathName.Pictures), "fsotobombth")
    dir

let mkdir path = 
    fs.stat(path, System.Func<_,_,_>(fun err stats ->
        if (not (isNull err) && err.code <> Some "ENOENT") then
            logError(err.ToString())
        elif (not (isNull err) || not (stats.isDirectory())) then
            fs.mkdir(path)
        else
            printfn "mkdir error: %A" err
        obj()
    ))

let cache (imgPath: string) = 
    imageCache.Insert(0, imgPath)

let getFromCache (index:int) =
    imageCache.[index]

let rm index donefn =
    fs.unlink(imageCache.[index], System.Func<_,_>(fun err ->
        match err with
        | null ->
            imageCache.RemoveAt(index)
            donefn()
        | _ -> logError(err.ToString())
    ))
