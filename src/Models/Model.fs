namespace Model

open Fable.Core

module Model =
    type Todo =
        { Id: int
          Text: string
          IsComplete: bool }

    type AppModel =
        { InputValue: string
          Todos: Todo list }
