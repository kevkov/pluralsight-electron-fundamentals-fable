#r "../node_modules/fable-core/Fable.Core.dll"
#load "./Fable.Import.Navigator.fsx"

open Fable.Core.JsInterop
open Fable.Import.Browser
open Fable.Import.Navigator_Extensions
open Fable.Import.Navigator_Extensions.UserMediaConstraints

let handleSuccess (videoEl: HTMLVideoElement) (stream: obj) = 
    videoEl.src <- window.URL.createObjectURL(stream)

let handleError err = 
    printfn "%A" err

let initVideo (navigator:Navigator) (videoEl:HTMLElement) =
    let constraints = createEmpty<Constraints>
    constraints.audio <- false
    constraints.video <- createEmpty<VideoConstraints>
    constraints.video.mandatory <- createEmpty<VideoMandatory>
    constraints.video.mandatory.minWidth <- 853
    constraints.video.mandatory.minHeight <- 480
    constraints.video.mandatory.maxWidth <- 853
    constraints.video.mandatory.maxHeight <- 480
    let success = handleSuccess (videoEl :?> HTMLVideoElement)  
    (navigator :?> Chrome).webkitGetUserMedia constraints (fun stream -> handleSuccess (videoEl :?> HTMLVideoElement) stream) handleError

let captureBytesFromLiveCanvas (canvasEl: HTMLCanvasElement) =
    canvasEl.toDataURL("image/png")

let captureBytes videoEl (ctx: CanvasRenderingContext2D) (canvasEl: HTMLCanvasElement) =
    ctx.drawImage(videoEl, 0., 0.)
    canvasEl.toDataURL("image/png")
