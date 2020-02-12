BEGIN
    DECLARE @SqlStatement NVARCHAR(500)

    SET @SqlStatement = 'GRANT ' + @Permission + ' ON [' + @SchemaName + '].[' + @ObjectName + '] TO [' + @UserName + ']'
	EXEC sp_executesql @SqlStatement
END