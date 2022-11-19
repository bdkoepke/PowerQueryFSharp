module Server

open Fable.Remoting.Server
open Fable.Remoting.AspNetCore

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting

open Shared
open System

let powerQueryApi =
    { evaluate =
        fun (formula, query, credentials) ->
            async {
                return PowerQuery.evaluateQuery(formula.Replace("\n", "\r\n"), query, credentials)
            }}

let webApp =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue powerQueryApi

let configureApp (app : IApplicationBuilder) =
    // Add the API to the ASP.NET Core pipeline
    app.UseRemoting(webApp)

[<EntryPoint>]
let main _ =
    WebHostBuilder()
        .UseKestrel()
        .Configure(Action<IApplicationBuilder> configureApp)
        .Build()
        .Run()
    0