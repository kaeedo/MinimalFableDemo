namespace Message

type TodoMessage =
| Create
| Complete of int
| Write of string
