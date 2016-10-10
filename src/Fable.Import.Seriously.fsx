#r "../node_modules/fable-core/Fable.Core.dll"
namespace Fable.Import
open System
open Fable.Core
open Fable.Import.JS
open Fable.Import.Node
open Fable.Import.Browser

module Seriously =

    type Target =
        abstract member source: obj with get, set

    [<AbstractClass>]
    type Seriously() =
         abstract member source: string -> obj
         abstract member target: string -> Target
         abstract member go: unit -> unit
        
    type SeriouslyCtor =
        [<Emit("new Seriously()")>] abstract Create: unit -> Seriously

    type Globals =
        static member Seriously: SeriouslyCtor = failwith "JSOnly"


