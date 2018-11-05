module App

open Elmish
open Elmish.React
open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser
open Fable.Helpers.React
open Fable.Helpers.React.Props

type Page =
| Counter of Counter.State
| Loader of Loader.State

type Model =
    { CurrentPage: Page
      Query: string }

type Message =
| CounterMessage of Counter.Message
| LoaderMessage of Loader.Message

let urlUpdate (result: Option<Router.Page>) model =
    match result with
    | None ->
        model, Navigation.modifyUrl "#"
    | Some (Router.Page.Counter startValue) ->
        let initialState, initialCmd = Counter.init startValue
        let nextState = { CurrentPage = (Page.Counter initialState); Query = startValue.ToString() }
        let nextCmd = Cmd.map CounterMessage initialCmd
        nextState, nextCmd
    | Some (Router.Page.Loader) ->
        let initialState, initialCmd = Loader.init()
        let nextState = { CurrentPage = Page.Loader initialState; Query = "" }
        let nextCmd = Cmd.map LoaderMessage initialCmd
        nextState, nextCmd

let init result =
    let initialState, _ = Counter.init 0
    urlUpdate result { CurrentPage = (Page.Counter initialState); Query = "0"}

let update msg state =
    match msg, state.CurrentPage with
    | CounterMessage counterMessage, Page.Counter subState ->
        let nextCounterState, nextCounterCmd = Counter.update counterMessage subState
        let nextState = { state with CurrentPage = (Page.Counter nextCounterState) }
        nextState, Cmd.map CounterMessage nextCounterCmd
    | LoaderMessage loaderMessage, Page.Loader loaderState ->
        let nextLoaderState, nextLoaderCmd = Loader.update loaderMessage loaderState
        let nextState = { state with CurrentPage = (Page.Loader nextLoaderState) }
        nextState, Cmd.map LoaderMessage nextLoaderCmd
    | _ ->
        state, Cmd.none

let divider =
    div [ Style [ MarginTop 20; MarginBottom 20 ] ] [ ]


let spinner =
    div [ Style [ Padding 20 ] ] [
        i [ ClassName "fa fa-circle-notch fa-spin fa-2x" ] [ ]
    ]

let render state dispatch =
    let navItem title =
        li [ ClassName "nav-item" ] [
            a [ Href (sprintf "#/%s" title) ]
              [ str title ]
        ]

    let currentPage =
        match state with
        | { CurrentPage = (Page.Counter counterState) } ->
            Counter.view counterState (CounterMessage >> dispatch)
        | { CurrentPage = (Page.Loader loaderState) } ->
            Loader.view loaderState (LoaderMessage >> dispatch)

    div [ Style [ Padding 20 ] ] [
        divider

        ul [ ClassName "nav nav-tabs" ] [
            navItem "counter/3"
            navItem "loader"
        ]

        divider
        currentPage
    ]

Program.mkProgram init update render
|> Program.toNavigable (parseHash Router.parser) urlUpdate
|> Program.withReact "elmish-app"
|> Program.run
