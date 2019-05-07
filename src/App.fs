module App

open System
open Elmish
open Elmish.React
open Fable.React 
open Fable.React.Props

type State = { 
  TodoList: string list 
  NewTodo : string 
}

type Msg =
  | SetNewTodo of string 
  | AddNewTodo 
  
let init() = { 
  TodoList = [ "Learn F#" ]
  NewTodo = "" 
}

let update (msg: Msg) (state: State) =
  match msg with
  | SetNewTodo desc -> 
      { state with NewTodo = desc }
  
  | AddNewTodo when String.IsNullOrWhiteSpace state.NewTodo ->
      state 

  | AddNewTodo ->
      { state with 
          NewTodo = ""
          TodoList = List.append state.TodoList [state.NewTodo] }

let render (state: State) (dispatch: Msg -> unit) =
  div [ Style [ Padding 30 ] ] [
    p [ Class "title" ] [ str "Elmish To-Do list" ]
    // the text box to add new todo items
    div [ Class "field has-addons" ] [
      div [ Class "control is-expanded" ] [ 
        input [ 
          Class "input is-medium"
          valueOrDefault state.NewTodo
          OnChange (fun ev -> dispatch (SetNewTodo ev.Value)) 
        ]
      ] 
      div [ Class "control" ] [ 
        button [ Class "button is-primary is-medium"; OnClick (fun _ -> dispatch AddNewTodo) ] [ 
          i [ Class "fa fa-plus" ] [ ]
        ]
      ] 
    ] 
    // the actual todo items
    ul [ Style [ MarginTop 20 ] ] [ 
      for todo in state.TodoList -> 
      li [ Class "box" ] [ 
        p [ Class "subtitle" ] [ str todo ] 
      ]  
    ]
  ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run