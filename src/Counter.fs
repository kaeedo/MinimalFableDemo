module Counter

open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props

type State = { Count: int }

type Message =
| Increment
| Decrement

let init startValue = { Count = startValue }, Cmd.none

let update message state =
    match message with
    | Increment ->
        let nextState = { state with Count = state.Count + 1}
        nextState, Cmd.none
    | Decrement ->
        let nextState = { state with Count = state.Count - 1}
        nextState, Cmd.none

let incrementHandler dispatch _ =
    dispatch Increment

let decrementHandler dispatch _ =
    dispatch Decrement

let view state dispatch =
    div [] [
        button [ OnClick <| incrementHandler dispatch ] [ str "Increment" ]
        button [ OnClick <| decrementHandler dispatch ] [ str "Decrement" ]
        h1 [] [ str <| state.Count.ToString() ]
    ]

