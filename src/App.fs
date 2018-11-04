module App

open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props

type Page =
| Counter of Counter.State
| Loader of Loader.State

type State = CurrentPage of Page

type Url = Url of string

type Message =
| CounterMessage of Counter.Message
| LoaderMessage of Loader.Message
| NavigateTo of string


let init() =
    let initialCounterState, initialCounterCmd = Counter.init()
    let inisitalState = CurrentPage (Page.Counter initialCounterState)
    let initialCmd = Cmd.map CounterMessage initialCounterCmd
    inisitalState, initialCmd

let update msg state =
    match state, msg with
    | CurrentPage (Page.Counter counterState), CounterMessage counterMessage ->
        let nextCounterState, nextCounterCmd = Counter.update counterMessage counterState
        let nextState = CurrentPage (Page.Counter nextCounterState)
        nextState, Cmd.map CounterMessage nextCounterCmd
    | CurrentPage (Page.Loader loaderState), LoaderMessage loaderMessage ->
        let nextLoaderState, nextLoaderCmd = Loader.update loaderMessage loaderState
        let nextState = CurrentPage (Page.Loader nextLoaderState)
        nextState, Cmd.map LoaderMessage nextLoaderCmd
    | _, NavigateTo "/counter" ->
        let initialCounterState, initialCounterCmd = Counter.init()
        let nextState = CurrentPage (Page.Counter initialCounterState)
        let nextCmd = Cmd.map CounterMessage initialCounterCmd
        nextState, nextCmd
    | _, NavigateTo "/loader" ->
        let initialLoaderState, initialLoaderCmd = Loader.init()
        let nextState = CurrentPage (Page.Loader initialLoaderState)
        let nextCmd = Cmd.map LoaderMessage initialLoaderCmd
        nextState, nextCmd
    | _, _ ->
        let initialCounterState, initialCounterCmd = Counter.init()
        let nextState = CurrentPage (Page.Counter initialCounterState)
        let nextCmd = Cmd.map CounterMessage initialCounterCmd
        nextState, nextCmd

let divider =
    div [ Style [ MarginTop 20; MarginBottom 20 ] ] [ ]


let spinner =
    div [ Style [ Padding 20 ] ] [
        i [ ClassName "fa fa-circle-notch fa-spin fa-2x" ] [ ]
    ]

let render state dispatch =
    let currentUrl =
        match state with
        | CurrentPage (Page.Counter _ ) -> "/counter"
        | CurrentPage (Page.Loader _) -> "/loader"

    let navItem nextUrl title =
        let notActive = currentUrl <> nextUrl
        let navLinkClass = if notActive then "nav-link" else "nav-link active"
        li [ ClassName "nav-item" ] [
            a [ ClassName navLinkClass
                Href "#"
                OnClick (fun _ -> dispatch (NavigateTo nextUrl)) ]
              [ str title ]
        ]

    let currentPage =
        match state with
        | CurrentPage (Page.Counter counterState) ->
            Counter.view counterState (CounterMessage >> dispatch)
        | CurrentPage (Page.Loader loaderState) ->
            Loader.view loaderState (LoaderMessage >> dispatch)

    div [ Style [ Padding 20 ] ] [
        divider

        ul [ ClassName "nav nav-tabs" ] [
            navItem "/counter" "Counter"
            navItem "/loader" "Loader"
        ]

        divider
        currentPage
    ]


Program.mkProgram init update render
|> Program.withReact "elmish-app"
|> Program.run
