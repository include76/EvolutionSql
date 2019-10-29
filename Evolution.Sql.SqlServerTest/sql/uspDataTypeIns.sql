CREATE PROCEDURE uspDataTypeIns(
	@ColBigInt		BIGINT = NULL,
	@ColBit			BIT = NULL,
	@ColDecimal		DECIMAL(18,4) = NULL,
	@ColInt			INT = NULL,
	@ColMoney		MONEY = NULL,
	@ColNumeric		NUMERIC(18,4) = NULL,
	@ColSmallInt	SMALLINT = NULL,
	@ColSmallMoney	SMALLMONEY = NULL,
	@ColTinyInt		TINYINT = NULL,
	@ColFloat		FLOAT = NULL,
	@ColReal		REAL = NULL,
	@ColDate		DATE = NULL,
	@ColDatetime	DATETIME = NULL,
	@ColDatetime2	DATETIME2 = NULL,
	@ColDatetimeOffset	DATETIMEOFFSET = NULL,
	@ColSmallDatetime	SMALLDATETIME = NULL,
	@ColTime		TIME = NULL,
	@ColChar		CHAR(1000) = NULL,
	@ColText		TEXT = NULL, --will be removed in future version SQL Server
	@ColVarchar		VARCHAR(1000) = NULL,
	@ColNChar		NCHAR(1000) = NULL,
	@ColNText		NTEXT = NULL, --will be removed in future version SQL Server
	@ColNVarchar	NVARCHAR(max) = NULL,
	@ColBinary		BINARY(1000) = NULL,
	@ColImage		IMAGE = NULL, --will be removed in future version SQL Server
	@ColVarBinary	VARBINARY(max) = NULL,
	@ColXml			XML = NULL
)
AS
BEGIN
	INSERT INTO DataTypeTable VALUES(
		@ColBigInt	,
		@ColBit		,
		@ColDecimal	,
		@ColInt		,
		@ColMoney	,
		@ColNumeric	,
		@ColSmallInt	,
		@ColSmallMoney	,
		@ColTinyInt	,
		@ColFloat	,
		@ColReal		,
		@ColDate		,
		@ColDatetime ,
		@ColDatetime2 ,
		@ColDatetimeOffset	,
		@ColSmallDatetime	,
		@ColTime		,
		@ColChar		,
		@ColText		, 
		@ColVarchar	,
		@ColNChar	,
		@ColNText	, 
		@ColNVarchar,
		@ColBinary	,
		@ColImage	, 
		@ColVarBinary ,
		@ColXml		,
		default
	)
END