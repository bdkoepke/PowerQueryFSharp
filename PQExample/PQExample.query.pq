// Use this file to write queries to test your data connector
let
    etfdb = MySQL.Database("mysql.alphaarchitect.com", "etfdb", [ReturnSingleDatabase=true]),
    world = "World",
    brandon = [Age=32, First ="Brandon"],
    people = #table(type table [Age=Int64.Type, Name=text], {{32, "Brandon"}, {29, "Yan"}, {41, "Wes"}})
in
    people