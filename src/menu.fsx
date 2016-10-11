#r "../node_modules/fable-core/Fable.Core.dll"
#load "../node_modules/fable-import-electron/Fable.Import.Electron.fs"

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Electron

let menuTemplate (mainWindow: BrowserWindow) = 
    
    let cycle = createEmpty<MenuItemOptions>
    cycle.label <- Some "Cycle"
    cycle.accelerator <- Some "Shift+CmdOrCtrl+E"

    let sep = createEmpty<MenuItemOptions>
    sep.``type`` <- Some MenuItemType.Separator

    let vanillafn (item: MenuItem) (window: BrowserWindow) =
        mainWindow.webContents.send("effect-choose")
    let vanilla = createEmpty<MenuItemOptions>
    vanilla.label <- Some "None"
    vanilla.``type`` <- Some MenuItemType.Radio
    vanilla.click <- Some (Func<_,_,_> vanillafn)

    let ascii = createEmpty<MenuItemOptions>
    ascii.label <- Some "Ascii"
    ascii.``type`` <- Some MenuItemType.Radio
    ascii.click <- Some (Func<_,_,_> (fun (item: MenuItem) (window: BrowserWindow) ->
        mainWindow.webContents.send("effect-choose", "ascii")))

    let daltonize = createEmpty<MenuItemOptions>
    daltonize.label <- Some "Daltonize"
    daltonize.``type`` <- Some MenuItemType.Radio
    daltonize.click <- Some (Func<_,_,_> (fun (item: MenuItem) (window: BrowserWindow) ->
        mainWindow.webContents.send("effect-choose", "daltonize")))

    let hex = createEmpty<MenuItemOptions>
    hex.label <- Some "Hex"
    hex.``type`` <- Some MenuItemType.Radio
    hex.click <- Some (Func<_,_,_> (fun (item: MenuItem) (window: BrowserWindow) ->
        mainWindow.webContents.send("effect-choose", "hex")))

    let effectsItems = ResizeArray<MenuItemOptions> ()
    effectsItems.Add cycle
    effectsItems.Add sep
    effectsItems.Add vanilla
    effectsItems.Add ascii
    effectsItems.Add daltonize
    effectsItems.Add hex
    
    let effects = createEmpty<MenuItemOptions>
    effects.label <- Some "Effects"
    effects.submenu <- U2.Case2 effectsItems |> Some

    let menuOptions = ResizeArray<MenuItemOptions>()
    menuOptions.Add effects
    menuOptions