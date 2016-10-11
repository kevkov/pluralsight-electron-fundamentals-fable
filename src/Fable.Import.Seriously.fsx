#r "../node_modules/fable-core/Fable.Core.dll"
namespace Fable.Import
open System
open Fable.Core
open Fable.Import.JS
open Fable.Import.Node
open Fable.Import.Browser

module Seriously =


    type Node =
        abstract member source: Node with get, set

    [<AbstractClass>]
    type Seriously() =
         abstract member source: string -> Node
         abstract member target: string -> Node
         abstract member go: unit -> unit
         abstract member effect: string -> Node
        
    type SeriouslyCtor =
        [<Emit("new Seriously()")>] abstract Create: unit -> Seriously

    type Globals =
        static member Seriously: SeriouslyCtor = failwith "JSOnly"


