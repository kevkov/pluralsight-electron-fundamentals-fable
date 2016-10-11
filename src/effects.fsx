#r "../node_modules/fable-core/Fable.Core.dll"
#load "./Fable.Import.Seriously.fsx"

open Fable.Core.JsInterop
open Fable.Import.Seriously

type Effector = Seriously -> Node -> Node -> unit

type Effection = {name: string; effect: Effector}

let connectEffect (seriously: Seriously) (source: Node) (target: Node) (effect: Node) =
    effect.source <- source
    target.source <- effect
    seriously.go ()

let vanilla (seriously: Seriously) (source: Node) (target: Node) =
    target.source <- source
    seriously.go ()

let ascii (seriously: Seriously) (source: Node) (target: Node) =
    let effect = seriously.effect "ascii"
    connectEffect seriously source target effect

let daltonize (seriously: Seriously) (source: Node) (target: Node) =
    let effect = seriously.effect "daltonize"
    effect?``type`` <- "0.8"
    connectEffect seriously source target effect

let hex (seriously: Seriously) (source: Node) (target: Node) =
    let effect = seriously.effect "hex"
    effect?size <- 0.03
    connectEffect seriously source target effect
    
let effects = [
                {name = "vanilla"; effect = vanilla}
                {name = "ascii"; effect = ascii }
                {name = "daltonize"; effect = daltonize }
                {name = "hex"; effect = hex }
            ]

let chooseEffect seriously source target effectName = 
    let effect = effects |> List.tryFind (fun l -> l.name = effectName)
    match effect with
    | Some e -> printfn "effect"; e.effect seriously source target
    | None -> vanilla seriously source target