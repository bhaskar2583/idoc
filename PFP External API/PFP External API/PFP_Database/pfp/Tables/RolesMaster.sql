CREATE TABLE [pfp].[RolesMaster] (
    [ROM_Id]          INT           IDENTITY (1, 1) NOT NULL,
    [ROM_Name]        VARCHAR (255) NOT NULL,
    [ROM_Description] VARCHAR (255) NULL,
    [ROM_Active]      BIT           CONSTRAINT [DF_RolesMaster_ROM_Active] DEFAULT ((1)) NOT NULL,
    [ROM_CreatedBy]   VARCHAR (50)  NOT NULL,
    [ROM_CreatedOn]   DATETIME      NOT NULL,
    [ROM_UpdatedBy]   VARCHAR (50)  NOT NULL,
    [ROM_UpdatedOn]   DATETIME      NOT NULL,
    [ROM_IsDeleted]   BIT           CONSTRAINT [DF_RolesMaster_ROM_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_RolesMaster] PRIMARY KEY CLUSTERED ([ROM_Id] ASC)
);

