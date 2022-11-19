// This file contains your Data Connector logic
section PQExample;

[DataSource.Kind="PQExample", Publish="PQExample.Publish"]
shared PQExample.Contents = (optional message as text) =>
    let
        _message = if (message <> null) then message else "(no message)",
        a = "Hello from PQExample: " & _message
    in
        a;

// Data Source Kind description
PQExample = [
    Authentication = [
        // Key = [],
        // UsernamePassword = [],
        // Windows = [],
        Implicit = []
    ],
    Label = Extension.LoadString("DataSourceLabel")
];

// Data Source UI publishing description
PQExample.Publish = [
    Beta = true,
    Category = "Other",
    ButtonText = { Extension.LoadString("ButtonTitle"), Extension.LoadString("ButtonHelp") },
    LearnMoreUrl = "https://powerbi.microsoft.com/",
    SourceImage = PQExample.Icons,
    SourceTypeImage = PQExample.Icons
];

PQExample.Icons = [
    Icon16 = { Extension.Contents("PQExample16.png"), Extension.Contents("PQExample20.png"), Extension.Contents("PQExample24.png"), Extension.Contents("PQExample32.png") },
    Icon32 = { Extension.Contents("PQExample32.png"), Extension.Contents("PQExample40.png"), Extension.Contents("PQExample48.png"), Extension.Contents("PQExample64.png") }
];