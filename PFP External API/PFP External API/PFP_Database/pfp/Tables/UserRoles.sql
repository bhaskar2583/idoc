CREATE TABLE [pfp].[UserRoles] (
    [URS_Id]        INT          IDENTITY (1, 1) NOT NULL,
    [URS_UserId]    INT          NOT NULL,
    [URS_RoleId]    INT          NOT NULL,
    [URS_CreatedBy] VARCHAR (50) NULL,
    [URS_CreatedOn] DATETIME     NULL,
    [URS_UpdatedBy] VARCHAR (50) NULL,
    [URS_UpdatedOn] DATETIME     NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([URS_Id] ASC)
);

