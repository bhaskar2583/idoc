CREATE TABLE [pfp].[Users] (
    [USR_Id]               INT           IDENTITY (1, 1) NOT NULL,
    [USR_FirstName]        VARCHAR (50)  NULL,
    [USR_LastName]         VARCHAR (50)  NULL,
    [USR_Email]            VARCHAR (100) NULL,
    [USR_Phone]            VARCHAR (100) NULL,
    [USR_OrganizationName] VARCHAR (255) NULL,
    [USR_Active]           BIT           NULL,
    [USR_CreatedBy]        VARCHAR (50)  NULL,
    [USR_CreatedOn]        DATETIME      NULL,
    [USR_UpdatedBy]        VARCHAR (50)  NULL,
    [USR_UpdatedOn]        DATETIME      NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([USR_Id] ASC)
);

