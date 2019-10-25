
--https://docs.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-ver15
CREATE TABLE DataTypeTable(
	ColBigInt	BIGINT,
	ColBit		BIT,
	ColDecimal	DECIMAL(18,4),
	ColInt		INT,
	ColMoney	MONEY,
	ColNumeric	NUMERIC(18,4),
	ColSmallInt	SMALLINT,
	ColSmallMoney	SMALLMONEY,
	ColTinyInt	TINYINT,
	ColFloat	FLOAT,
	ColReal		REAL,
	ColDate		DATE,
	ColDatetime DATETIME,
	ColDatetime2 DATETIME2,
	ColDatetimeOffset	DATETIMEOFFSET,
	ColSmallDatetime	SMALLDATETIME,	
	ColTime		TIME,
	ColChar		CHAR(1000),
	ColText		TEXT, --will be removed in future version SQL Server
	ColVarchar	VARCHAR(1000),
	ColNChar	NCHAR(1000),
	ColNText	NTEXT, --will be removed in future version SQL Server
	ColNVarchar	NVARCHAR(max),
	ColBinary	BINARY(1000),
	ColImage	IMAGE, --will be removed in future version SQL Server
	ColVarBinary VARBINARY(max),
	ColXml		XML,
	ColTimestamp		TIMESTAMP,
)