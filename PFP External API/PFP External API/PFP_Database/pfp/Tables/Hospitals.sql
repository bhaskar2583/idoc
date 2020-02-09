CREATE TABLE [pfp].[Hospitals] (
    [HOS_Id]           INT           IDENTITY (1, 1) NOT NULL,
    [HOS_PFI]          INT           NULL,
    [HOS_Unique_Id]    VARCHAR (50)  NOT NULL,
    [HOS_HospitalName] VARCHAR (250) NOT NULL,
    [HOS_Parent_Id]    VARCHAR (50)  NOT NULL,
    [HOS_Active]       BIT           NOT NULL,
    [HOS_CreatedBy]    VARCHAR (250) NOT NULL,
    [HOS_CreatedOn]    DATETIME      NOT NULL,
    [HOS_UpdatedBy]    VARCHAR (250) NOT NULL,
    [HOS_UpdatedOn]    DATETIME      NOT NULL,
    CONSTRAINT [PK_Hospitals] PRIMARY KEY CLUSTERED ([HOS_Id] ASC)
);

