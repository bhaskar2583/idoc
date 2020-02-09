CREATE TABLE [pfp].[MeasureMaster] (
    [MEM_Id]         INT           IDENTITY (1, 1) NOT NULL,
    [MEM_Measure]    VARCHAR (200) NOT NULL,
    [MEM_Multiplier] INT           NOT NULL,
    [MEM_Active]     BIT           CONSTRAINT [DF__MeasureMa__MEM_A__7B5B524B] DEFAULT ((1)) NOT NULL,
    [MEM_CreatedBy]  VARCHAR (50)  NOT NULL,
    [MEM_CreatedOn]  DATETIME      CONSTRAINT [DF__MeasureMa__MEM_C__7C4F7684] DEFAULT (getdate()) NOT NULL,
    [MEM_UpdatedBy]  VARCHAR (50)  NOT NULL,
    [MEM_UpdatedOn]  DATETIME      NOT NULL,
    CONSTRAINT [PK_MEM_Id_MeasureMaster] PRIMARY KEY CLUSTERED ([MEM_Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is Measure master table''s primary key', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'MeasureMaster', @level2type = N'COLUMN', @level2name = N'MEM_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is pfp Measure value', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'MeasureMaster', @level2type = N'COLUMN', @level2name = N'MEM_Measure';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Active status for Measure', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'MeasureMaster', @level2type = N'COLUMN', @level2name = N'MEM_Active';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record created user''s id', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'MeasureMaster', @level2type = N'COLUMN', @level2name = N'MEM_CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record created date and time', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'MeasureMaster', @level2type = N'COLUMN', @level2name = N'MEM_CreatedOn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record updated/modified user''s id', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'MeasureMaster', @level2type = N'COLUMN', @level2name = N'MEM_UpdatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record updated/modified date and time', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'MeasureMaster', @level2type = N'COLUMN', @level2name = N'MEM_UpdatedOn';

