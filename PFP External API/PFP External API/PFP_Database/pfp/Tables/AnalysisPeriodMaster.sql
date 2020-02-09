CREATE TABLE [pfp].[AnalysisPeriodMaster] (
    [APM_Id]        INT          IDENTITY (1, 1) NOT NULL,
    [APM_Period]    VARCHAR (50) NOT NULL,
    [APM_Active]    BIT          DEFAULT ((1)) NOT NULL,
    [APM_CreatedBy] VARCHAR (50) NOT NULL,
    [APM_CreatedOn] DATETIME     DEFAULT (getdate()) NOT NULL,
    [APM_UpdatedBy] VARCHAR (50) NOT NULL,
    [APM_UpdatedOn] DATETIME     NOT NULL,
    CONSTRAINT [PK_APM_Id_AnalysisPeriodMaster] PRIMARY KEY CLUSTERED ([APM_Id] ASC) WITH (PAD_INDEX = ON)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is Analysis Period master table''s primary key', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'AnalysisPeriodMaster', @level2type = N'COLUMN', @level2name = N'APM_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is pfp Analysis Period value', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'AnalysisPeriodMaster', @level2type = N'COLUMN', @level2name = N'APM_Period';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Active status for Period', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'AnalysisPeriodMaster', @level2type = N'COLUMN', @level2name = N'APM_Active';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record created user''s id', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'AnalysisPeriodMaster', @level2type = N'COLUMN', @level2name = N'APM_CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record created date and time', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'AnalysisPeriodMaster', @level2type = N'COLUMN', @level2name = N'APM_CreatedOn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record updated/modified user''s id', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'AnalysisPeriodMaster', @level2type = N'COLUMN', @level2name = N'APM_UpdatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record updated/modified date and time', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'AnalysisPeriodMaster', @level2type = N'COLUMN', @level2name = N'APM_UpdatedOn';

