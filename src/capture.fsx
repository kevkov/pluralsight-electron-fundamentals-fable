#r "../node_modules/fable-core/Fable.Core.dll"
#load "../node_modules/fable-import-electron/Fable.Import.Electron.fs"
#load "./Fable.Import.Seriously.fsx"
#load "./countdown.fsx"
#load "./video.fsx"
#load "./flash.fsx"
#load "./effects.fsx"

open Fable.Import.Seriously
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser
open Fable.Import.Electron
open Fable.Import.electron_Extensions
open Fable.Import.Node
open Video
open Countdown
open Flash
open Effects

let findIndex predicate (nodelist: NodeListOf<'a>) =
    seq { for a in 0 .. int nodelist.length do
            yield nodelist.Item(a)  }
    |> Seq.findIndex predicate

let formatImgTag (doc:Document) (bytes:string) =
    let div = doc.createElement("div")
    div.classList.add("photo")
    let close = doc.createElement("div")
    close.classList.add("photoClose")
    let img = doc.createElement_img()
    img.classList.add("photoImg")
    img.src <- bytes
    div.appendChild(img) |> ignore
    div.appendChild(close) |> ignore
    div

window.addEventListener("DOMContentLoaded", unbox (fun e ->
    let videoEl = document.getElementById("video") :?> HTMLVideoElement
    let recordEl = document.getElementById("record")
    let photosEl = document.querySelector(".photosContainer") :?> HTMLElement
    let counterEl = document.getElementById("counter")
    let canvasEl = document.getElementById("canvas") :?> HTMLCanvasElement
    let flashEl = document.getElementById("flash")

    let seriously = Globals.Seriously.Create () 
    let videoSrc = seriously.source "#video"
    let canvasTarget = seriously.target "#canvas"

    chooseEffect seriously videoSrc canvasTarget "vanilla"
    
    initVideo navigator videoEl

    recordEl.addEventListener_click(System.Func<_,_>(fun evt ->
        countdown counterEl 3  (fun _ ->
            flash flashEl
            let bytes = captureBytesFromLiveCanvas canvasEl
            //let bytes = captureBytes (U3.Case3 videoEl) ctx canvasEl
            electron.ipcRenderer.send("image-captured", bytes)
            photosEl.appendChild(formatImgTag document bytes) |> ignore
        )
        null
    ) )

    photosEl.addEventListener_click(System.Func<_,_>(fun evt ->
        let isRm = (evt.target :?> Element).classList.contains "photoClose"
        let selector = if isRm then ".photoClose" else ".photoImg"

        let photos = document.querySelectorAll(selector)
        let index = photos |> findIndex(fun el -> el = (evt.target :?> Element))
        if index > -1 then
            if isRm then
                electron.ipcRenderer.send("image-remove", index)
            else
                let rmain = electron.remote.require "./main"
                electron.shell.showItemInFolder (string (rmain?getImageFromCache index))
        null        
    ))

    electron.ipcRenderer.on("effect-choose", (fun (evt:IpcRendererEvent) effectName ->
        chooseEffect seriously videoSrc canvasTarget (string effectName)
        
    )) |> ignore

    electron.ipcRenderer.on("effect-cycle", (fun (evt:IpcRendererEvent) data ->
        //cycleEffects seriously videoSrc canvasTarget
        ()
    ))

))

electron.ipcRenderer.on("image-removed", (fun (evt:IpcRendererEvent) index ->
    let container = document.getElementById("photos")
    let allphotos = document.querySelectorAll(".photo")
    container.removeChild(allphotos.Item(int (string index))) |> ignore
))

