namespace Shared

open System.Data

type ODataCredential =
    | Anonymous
    | UsernamePassword of Username: string * Password: string

and SqlCredential =
    | Windows
    | UsernamePassword of Username: string * Password: string

and WebCredential =
    | Anonymous
    | UsernamePassword of Username: string * Password: string

and Credential =
    | File of Path: string
    | Folder of Path: string
    | OData of Url: string * ODataCredential
    | Sql of SQL: string * SqlCredential
    | Web of Url: string * WebCredential

type Formula = string
type Query = string

type IPowerQueryApi =
    { evaluate: Formula * Query * list<Credential> -> Async<DataTable> }

module Route =
    let builder (_: string) (methodName: string) = 
        sprintf "/api/%s" methodName