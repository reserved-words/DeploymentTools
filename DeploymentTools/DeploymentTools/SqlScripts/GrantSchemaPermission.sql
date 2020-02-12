BEGIN
    DECLARE @SqlStatement NVARCHAR(500)

    SET @SqlStatement = 'GRANT ' + @Permission + ' ON SCHEMA::' + @SchemaName + ' TO [' + @UserName + ']'
	EXEC sp_executesql @SqlStatement
END