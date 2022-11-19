module PowerQuery
    open Microsoft.Data.Mashup
    open Microsoft.Mashup.Tools
    open Shared

    let private createMashupConnectionInfo (formula, query) =
        let (mcsb, error) = Utilities.CreateMashupConnectionInfo(formula)
        match mcsb, error with
        | null, null -> None
        | x, null -> x.Location <- query
                     Some mcsb
        | _, _ -> failwith error.Message

    let private evaluate(mcsb: MashupConnectionStringBuilder, creds: CredentialStore) =
        let queryExecutor = QueryExecutor(AllowNativeQuery = true, FastCombine = true)
        let task = queryExecutor.CreateExecution(mcsb, creds, true)
        task.RunSynchronously()
        match task.Exception, task.Result.Error with
        | null, null ->
            let table = task.Result.Table
            table.TableName <- mcsb.Location
            table
        | e, null -> failwith e.Message
        | _, e ->  failwith e.Message

    let private setCredential (s: CredentialStore) (credential: Credential) =
        match credential with
        | File path -> s.SetCredential(DataSource(DataSourceKind.File, path), DataSourceSetting.CreateWindowsCredential(), null)
        | Folder path -> s.SetCredential(DataSource(DataSourceKind.Folder, path), DataSourceSetting.CreateWindowsCredential(), null)
        | Web (url, c) ->
            let d = match c with
                    | Anonymous -> DataSourceSetting.CreateAnonymousCredential()
                    | UsernamePassword (u, p) -> DataSourceSetting.CreateUsernamePasswordCredential(u, p)
            s.SetCredential(DataSource(DataSourceKind.Web, url), d, null)
        | OData (url, c) ->
            let d = match c with
                    | ODataCredential.Anonymous -> DataSourceSetting.CreateAnonymousCredential()
                    | ODataCredential.UsernamePassword (u, p) -> DataSourceSetting.CreateUsernamePasswordCredential(u, p)
            s.SetCredential(DataSource(DataSourceKind.OData, url), d, null)
        | Sql (sql, c) ->
            let d = match c with
                    | Windows -> DataSourceSetting.CreateWindowsCredential()
                    | SqlCredential.UsernamePassword (u, p) -> DataSourceSetting.CreateUsernamePasswordCredential(u, p)
            s.SetCredential(DataSource(DataSourceKind.Sql, sql), d, null)
        s

    let private createCredentialStore (credentials: list<Credential>) =
        credentials
        |> List.fold setCredential (CredentialStore())

    let evaluateQuery(formula, query, credentials: list<Credential>) =
        let results = evaluate(createMashupConnectionInfo(formula, query) |> Option.get, createCredentialStore credentials)

        MashupConnection.TryCleanup()
        |> ignore

        results