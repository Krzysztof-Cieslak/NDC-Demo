//`controller<'Key>` CE is higher level abstraction It will create
// complex routing for predefined set of operations which looks like this:
// [
//     GET [
//         route "/" index
//         route "/add" add
//         routef "/%?" show
//         routef "/%?/edit" edit
//     ]
//     POST [
//         route "/" create
//     ]
//     PUT [
//         route "/%?" update
//     ]
//     PATCH [
//         route "/%?" update
//     ]
//     DELETE [
//         route "/%?" delete
//     ]
// ]
let userController = controller {
    not_found_handler (text "Users 404")

    index (fun ctx -> "Index handler" |> Controller.text ctx)
    add (fun ctx -> "Add handler" |> Controller.text ctx)
    show (fun (ctx, id) -> (sprintf "Show handler - %s" id) |> Controller.text ctx)
    edit (fun (ctx, id) -> (sprintf "Edit handler - %s" id) |> Controller.text ctx)
}

let topRouter = scope {
    pipe_through headerPipe
    not_found_handler (text "404")

    // or can be defined separatly and used as HttpHandler
    forward "/api" apiRouter

    // same with controllers
    forward "/users" userController
}