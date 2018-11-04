module Loader

open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props

module Cmd =
    let afterTimeout n (msg: 't) : Cmd<'t> =
        [ fun dispatch ->
            async {
                do! Async.Sleep n
                do (dispatch msg)
            }
            |> Async.StartImmediate
        ]

type LoaderState =
| Initial
| Loading
| Loaded of string

type State = { Loader: LoaderState }

type Message =
| StartLoading
| FinishedLoading of string
| ResetLoader

let init() = { Loader = LoaderState.Initial }, Cmd.none

let update message state =
    match message with
    | StartLoading ->
        let nextState = { state with Loader = LoaderState.Loading }
        let nextCmd = Cmd.afterTimeout 2000 (FinishedLoading "Remote")
        nextState, nextCmd
    | FinishedLoading data ->
        let nextState = { state with Loader = LoaderState.Loaded data }
        nextState, Cmd.none
    | ResetLoader ->
        let nextState = { state with Loader = LoaderState.Initial }
        nextState, Cmd.none

let startLoadingHandler dispatch _ = dispatch StartLoading

let resetHandler dispatch _ = dispatch ResetLoader

let view state dispatch =
    match state.Loader with
    | Initial ->
         button [ OnClick <| startLoadingHandler dispatch ] [ str "Start Loading" ]
    | Loading -> h3 [] [ str "LOADING..." ]
    | Loaded data ->
        div [ ] [
            h3 [ ] [ str data ]
            button [ OnClick <| resetHandler dispatch ] [ str "Start Loading" ]
        ]
