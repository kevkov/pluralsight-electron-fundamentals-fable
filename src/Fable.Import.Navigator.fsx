#r "../node_modules/fable-core/Fable.Core.dll"
#load "../node_modules/fable-import-electron/Fable.Import.Electron.fs"

namespace Fable.Import

open Fable.Import.Browser

module Navigator_Extensions =
    module UserMediaConstraints =
        type VideoMandatory = 
            abstract member minWidth: int with get, set
            abstract member maxWidth: int with get, set
            abstract member minHeight: int with get, set
            abstract member maxHeight: int with get, set

        type VideoConstraints =
            abstract member mandatory: VideoMandatory with get, set

        type Constraints =
            abstract member audio: bool with get, set
            abstract member video: VideoConstraints with get, set

        // type SuccessCallback = obj -> unit  
        // type ErrorCallback = obj -> unit

    open UserMediaConstraints    
    type Chrome =
        inherit Navigator
        //member __.webkitGetUserMedia (constraints: Constraints) (success: obj -> unit) (error: obj -> unit) : unit = failwith "JSOnly"
        abstract member webkitGetUserMedia: Constraints -> (obj -> unit) ->(obj -> unit) -> unit
        