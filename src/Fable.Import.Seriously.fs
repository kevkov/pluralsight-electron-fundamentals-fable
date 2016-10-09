namespace Fable.Import
open System
open Fable.Core
open Fable.Import.JS
open Fable.Import.Node
open Fable.Import.Browser

module Seriously =
    type Seriously() =
         member __.foo(): int = failwith "JS only"
         

[<AutoOpen>]
module Seriously_Extensions =
    let [<Global>] Seriously: unit -> Seriously.Seriously = failwith "JS only"

