BEGIN
    DECLARE @SqlStatement NVARCHAR(500)

    IF NOT EXISTS (SELECT [LoginName] FROM SYSLOGINS WHERE [Name] = @UserName)
    BEGIN
        SET @SqlStatement = 'CREATE LOGIN [' + @UserName + '] FROM WINDOWS WITH DEFAULT_DATABASE=[' + @DatabaseName + '], DEFAULT_LANGUAGE=[us_english]'
        EXEC sp_executesql @SqlStatement
    END

    IF NOT EXISTS (SELECT [Name] FROM SYSUSERS WHERE [Name] = @UserName)
    BEGIN
        SET @SqlStatement = 'CREATE USER [' + @UserName + '] FOR LOGIN [' + @UserName + '] WITH DEFAULT_SCHEMA = ' + @SchemaName
        EXEC sp_executesql @SqlStatement
    END

    SET @SqlStatement = 'GRANT CONNECT TO [' + @UserName + ']'
	EXEC sp_executesql @SqlStatement
END