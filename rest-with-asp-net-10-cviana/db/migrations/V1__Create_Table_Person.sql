IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[person]') AND type = 'U')
BEGIN
	CREATE TABLE dbo.person (
		[id] BIGINT NOT NULL IDENTITY,
		[first_name] VARCHAR(80) NOT NULL,
		[last_name] VARCHAR(80) NOT NULL,
		[address] VARCHAR(150) NOT NULL,
		[gender] VARCHAR(9) NOT NULL,
		PRIMARY KEY ([id])
	);
END