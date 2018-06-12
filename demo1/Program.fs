open System

open Saturn
open Giraffe

//Pipeline is declarative way to compose set of HttpHandlers
//Saturn provides set of popular HttpHandlers
let headerPipe = pipeline {
    set_header "myCustomHeader" "abcd"
    set_header "myCustomHeader2" "zxcv"
}

let endpointPipe = pipeline {
    //Plug any HttpHandler
    plug fetchSession
    plug head
    plug requestId

    plug headerPipe
}

//Scope is used to declar (sub)routing of your application. It also enables pluging common pipeline for set of routes.
let apiRouter = scope {
    pipe_through headerPipe
    not_found_handler (text "Api 404")

    get "/" (text "Hello world")
    get "/a" (text "Other hello world")
    deletef "/del/%s" (fun s -> text ("Deleted" + s))
}

let app = application {
    pipe_through endpointPipe

    router apiRouter
    url "http://0.0.0.0:8085/"
    memory_cache
}

[<EntryPoint>]
let main _ =
    app |> run
    0
