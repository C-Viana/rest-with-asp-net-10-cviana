IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[users]') AND type = 'U')
BEGIN
	CREATE TABLE dbo.users (
		[id] BIGINT NOT NULL IDENTITY,
		[user_name] VARCHAR(50) NOT NULL,
		[full_name] VARCHAR(250) NOT NULL,
		[password] VARCHAR(250) NOT NULL,
		[refresh_token] VARCHAR(500) NULL,
		[refresh_token_expiry_time] DATETIME2(6) NULL,
		CONSTRAINT UQ_users_user_name UNIQUE ([user_name]),
		PRIMARY KEY ([id])
	);
END