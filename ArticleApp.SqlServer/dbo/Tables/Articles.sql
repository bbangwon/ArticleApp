﻿-- 게시판 테이블
CREATE TABLE [dbo].[Articles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),	-- 일련번호
	[Title] NVARCHAR(255) NOT NULL,					-- 제목

	[CreatedBy] NVARCHAR(255) NULL,
	[Created] DATETIME DEFAULT(GETDATE()),
	[ModifiedBy] NVARCHAR(255) NULL,
	[Modified] DATETIME NULL,
)
GO