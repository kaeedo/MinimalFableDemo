module Loader

open Elmish
open Elmish.React
open Fable.Import.React
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Helpers.React.ReactiveComponents

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

type Message =
| StartLoading
| FinishedLoading of string
| ResetLoader

type State =
    { Loader: LoaderState
      Message: Message
      Dispatch: Message -> unit }

let init() = { Loader = LoaderState.Initial; Message = StartLoading; Dispatch = fun _ -> () }, Cmd.none

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

type Loader(props) =
    inherit Component<State, obj>(props)

    let dispatch = base.props.Dispatch

    let startLoadingHandler dispatch _ = dispatch StartLoading

    let resetHandler dispatch _ = dispatch ResetLoader

    override this.componentDidMount() =
        dispatch StartLoading

    override this.render() =
        let model = this.props.Loader

        match model with
        | Initial ->
             button [ OnClick <| startLoadingHandler dispatch ] [ str "Start Loading" ]
        | Loading -> h3 [] [ str "LOADING..." ]
        | Loaded data ->
            div [ ] [
                h3 [ ] [ str data ]
                button [ OnClick <| resetHandler dispatch ] [ str "Reset" ]
            ]
