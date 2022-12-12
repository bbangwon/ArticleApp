-- 게시판 테이블
CREATE TABLE [dbo].[Articles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),	-- 일련번호
	[Title] NVARCHAR(255) NOT NULL,					-- 제목
	[Content] NVARCHAR(MAX) NOT NULL,					-- 내용
	[IsPinned] Bit NULL DEFAULT(0),			-- 공지글로 올리기

	[CreatedBy] NVARCHAR(255) NULL,
	[Created] DATETIME DEFAULT(GETDATE()),
	[ModifiedBy] NVARCHAR(255) NULL,
	[Modified] DATETIME NULL,
)
GO
