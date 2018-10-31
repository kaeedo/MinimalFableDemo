module App

open Elmish
open Elmish.React
open Fable.Import
open Fable.Helpers.React
open Fable.Helpers.React.Props

open Model
open Message
open Command

let init(): Model.AppModel =
    { InputValue = ""
      Todos = [] }


let view (model: Model.AppModel) dispatch =
    let addTodo _ =
        dispatch <| Message.Create

    let inputOnChange (e: React.FormEvent) =
        let text = e.Value
        dispatch <| Message.Write text

    div []
        [ div []
              [ input [ Value model.InputValue; OnChange inputOnChange ]
                button [OnClick addTodo ] [ str "Add Todo" ] ]
          div [] [ str (string model) ] ]

Program.mkSimple init Command.update view
|> Program.withReact "elmish-app"
|> Program.withConsoleTrace
|> Program.run
