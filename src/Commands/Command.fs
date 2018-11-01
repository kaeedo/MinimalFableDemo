namespace Command

open System

open Message
open Model
open Model.Model


module Command =
    let update (message: Message.TodoMessage) (model: AppModel) =
        match message with
        | Create ->
            if String.IsNullOrWhiteSpace(model.InputValue)
            then model
            else
                let newTodo =
                    { Todo.Id = model.Todos.Length + 1
                      IsComplete = false
                      Text = model.InputValue }
                { model with
                    Todos = (newTodo :: model.Todos)
                    InputValue = "" }
        | Complete id -> model
        | Write text -> { model with InputValue = text}
