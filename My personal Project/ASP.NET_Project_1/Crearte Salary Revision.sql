
CREATE TABLE [dbo].[SalaryRevisions] (
    [RevisionId]  INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    [UserId]      INT NOT NULL,
    [OldSalary]   DECIMAL(18,2) DEFAULT 0 NOT NULL,
    [NewSalary]   DECIMAL(18,2) DEFAULT 0 NOT NULL,
    [RevisionDate] DATE DEFAULT GETDATE() NOT NULL,
    [Reason]      NVARCHAR(500) NULL,
    [ApprovedBy]  INT NULL,
    CONSTRAINT FK_SalaryRevisions_Users FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId]),
    CONSTRAINT FK_SalaryRevisions_ApprovedBy FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[Users]([UserId])
);

select * from SalaryRevisions