#r "../node_modules/fable-core/Fable.Core.dll"
#load "../node_modules/fable-import-electron/Fable.Import.Electron.fs"
#load "./images.fsx"

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Electron
open Images

// Keep a global reference of the window object, if you don't, the window will
// be closed automatically when the JavaScript object is garbage collected.
let mutable mainWindow: BrowserWindow option = Option.None

let createMainWindow () =
    let options = createEmpty<BrowserWindowOptions>
    options.width <- Some 1200.
    options.height <- Some 725.
    options.resizable <- Some false
    let window = electron.BrowserWindow.Create(options)

    // Load the index.html of the app.
    window.loadURL("file://" + Node.__dirname + "/../capture.html");

    #if DEBUG
    // fs.watch(Node.__dirname + "/renderer.js", fun _ ->
    //     window.webContents.reloadIgnoringCache() |> ignore
    // ) |> ignore

    #endif

    window.webContents.openDevTools()

    mkdir (getPicturesDir electron.app)

    // Emitted when the window is closed.
    window.on("closed", unbox(fun () ->
        // Dereference the window object, usually you would store windows
        // in an array if your app supports multi windows, this is the time
        // when you should delete the corresponding element.
        mainWindow <- Option.None
    )) |> ignore

    mainWindow <- Some window

let getImageFromCache (index:int) =
    printfn "getting %A" index
    getFromCache index


// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
electron.app.on("ready", unbox createMainWindow)

// Quit when all windows are closed.
electron.app.on("window-all-closed", unbox(fun () ->
    // On OS X it is common for applications and their menu bar
    // to stay active until the user quits explicitly with Cmd + Q
    if Node.``process``.platform <> "darwin" then
        electron.app.quit()
))

electron.app.on("activate", unbox(fun () ->
    // On OS X it's common to re-create a window in the app when the
    // dock icon is clicked and there are no other windows open.
    if mainWindow.IsNone then
        createMainWindow()
))

electron.ipcMain.on("image-captured", (fun (evt: IpcMainEvent) contents -> 
    saveImage (getPicturesDir electron.app) (contents.ToString()) cache
))

electron.ipcMain.on("image-remove", (fun (evt: IpcMainEvent) data ->
    let index = int (string data)
    rm index (fun _ -> 
        evt.sender.send("image-removed", index)
    )
))
