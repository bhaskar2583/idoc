CREATE TABLE [pfp].[NYSPFPData] (
    [NPD_Id]          INT             IDENTITY (1, 1) NOT NULL,
    [NPD_HOS_Id]      INT             NOT NULL,
    [NPD_Cal_Id]      INT             NOT NULL,
    [NPD_EMM_Id]      INT             NOT NULL,
    [NPD_Numerator]   DECIMAL (18, 9) NOT NULL,
    [NPD_Denominator] DECIMAL (18, 9) NOT NULL,
    [NPD_Measurement] DECIMAL (18, 9) NOT NULL,
    [NPD_APM_Id]      INT             NULL,
    [NPD_SourceType]  NVARCHAR (50)   NOT NULL,
    [NPD_CreatedBy]   VARCHAR (50)    NOT NULL,
    [NPD_CreatedOn]   DATETIME        NOT NULL,
    [NPD_UpdatedBy]   VARCHAR (50)    NOT NULL,
    [NPD_UpdatedOn]   DATETIME        NOT NULL,
    CONSTRAINT [PK_NYSPFPData] PRIMARY KEY CLUSTERED ([NPD_Id] ASC),
    CONSTRAINT [FK_NYSPFPData_AnalysisPeriodMaster] FOREIGN KEY ([NPD_APM_Id]) REFERENCES [pfp].[AnalysisPeriodMaster] ([APM_Id]),
    CONSTRAINT [FK_NYSPFPData_CalendarMaster] FOREIGN KEY ([NPD_Cal_Id]) REFERENCES [pfp].[CalendarMaster] ([CAL_Id]),
    CONSTRAINT [FK_NYSPFPData_EventMeasureMaster] FOREIGN KEY ([NPD_EMM_Id]) REFERENCES [pfp].[EventMeasureMaster] ([EMM_Id]),
    CONSTRAINT [FK_NYSPFPData_Hospitals] FOREIGN KEY ([NPD_HOS_Id]) REFERENCES [pfp].[Hospitals] ([HOS_Id])
);

