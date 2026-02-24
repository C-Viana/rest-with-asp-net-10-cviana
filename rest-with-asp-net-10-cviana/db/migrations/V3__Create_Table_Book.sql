IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[book]') AND type = 'U')
BEGIN
	CREATE TABLE dbo.book (
		[id] BIGINT NOT NULL IDENTITY,
		[author] VARCHAR(80) NOT NULL,
		[title] VARCHAR(250) NOT NULL,
		[launch_date] DATE NOT NULL,
		[price] DECIMAL(10, 2) NOT NULL,
		PRIMARY KEY ([id])
	);
END