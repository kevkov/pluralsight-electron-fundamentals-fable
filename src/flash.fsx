#r "../node_modules/fable-core/Fable.Core.dll"

open Fable.Import.Browser

let mutable private timer: float  = 0.

let flash (el: HTMLElement) =
    if el.classList.contains("is-flashing") then
        el.classList.remove("is-flashing")
    window.clearTimeout(timer)
    el.classList.add("is-flashing")
    timer <- window.setTimeout(System.Func<_,_>(fun _ ->  el.classList.remove("is-flashing")), 2000.)
    

