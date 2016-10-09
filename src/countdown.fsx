#r "../node_modules/fable-core/Fable.Core.dll"

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

let private setCount (counter: HTMLElement) count =
    counter.innerHTML <- if count > 0 then string count else ""

let countdown (counter: HTMLElement) downFrom donefn =
    [0..3] |> List.iter (fun i ->
        window.setTimeout((fun _ ->
            let count = downFrom - i
            setCount counter count
            if i = downFrom then donefn()
        ), i * 1000) |> ignore
    )