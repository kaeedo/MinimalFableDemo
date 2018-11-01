namespace Pages

open Fable.Import.React
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Model.Model
open Message

type MainProps =
    { Dispatch: TodoMessage -> unit
      Model: AppModel }

type Main(props) =
    inherit Component<MainProps, obj>(props)

    let dispatch = base.props.Dispatch
    let model = base.props.Model

    member private this.ChangeInput (e: FormEvent) =
        let text = e.Value

        dispatch <| Message.Write text

    member private this.AddTodo _ =
        dispatch <| Message.Create

    override this.render() =
        let model = this.props.Model

        div []
            [ div []
                  [ input [ Value model.InputValue; OnChange this.ChangeInput ]
                    button [ OnClick this.AddTodo ] [str "Add Todo" ] ] ]
