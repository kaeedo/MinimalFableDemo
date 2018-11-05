module Router

open Elmish.Browser.UrlParser

type Page =
| Counter of int
| Loader

let parser: Parser<Page -> Page, Page> =
    oneOf
        [ map Counter (s "counter" </> i32)
          map Loader (s "loader") ]
