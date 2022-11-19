module App

open Deedle
open Fable.Remoting.DotnetClient
open Shared

let pq =
    Remoting.createApi "http://localhost:5000/"
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<IPowerQueryApi>

[<EntryPoint>]
let main args =
    let formula = """section Section1;

shared person = #table(type table [First=text, Last=text, Age=number], {{"Brandon", "Koepke", 32}, {"Wes", "Gray", 41}});
    
shared accountBalance =
    let
        balance = 1000,
        deposit = 100,
        withdrawal = 500,
        interest = 50
     in
        balance + deposit + interest - withdrawal;"""

    let frame =
        let table = pq.evaluate(formula, "person", [])
                    |> Async.RunSynchronously
        Frame.ReadReader(table.CreateDataReader())
    frame.Print()
    0