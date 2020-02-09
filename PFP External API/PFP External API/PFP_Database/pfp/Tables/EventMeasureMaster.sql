CREATE TABLE [pfp].[EventMeasureMaster] (
    [EMM_Id]           INT          IDENTITY (1, 1) NOT NULL,
    [EMM_MEM_Id]       INT          NOT NULL,
    [EMM_EVM_Id]       INT          NOT NULL,
    [EMM_TimePeriod]   VARCHAR (50) NOT NULL,
    [EMM_Active]       BIT          CONSTRAINT [DF__EventMeas__EMM_A__3E1D39E1] DEFAULT ((1)) NOT NULL,
    [EMM_DisplayInApp] BIT          CONSTRAINT [DF_EventMeasureMaster_EMM_DisplayInApp] DEFAULT ((0)) NOT NULL,
    [EMM_CreatedBy]    VARCHAR (50) NOT NULL,
    [EMM_CreatedOn]    DATETIME     CONSTRAINT [DF__EventMeas__EMM_C__3F115E1A] DEFAULT (getdate()) NOT NULL,
    [EMM_UpdatedBy]    VARCHAR (50) NOT NULL,
    [EMM_UpdatedOn]    DATETIME     NOT NULL,
    CONSTRAINT [PK_EMM_Id_EventMeasureMaster] PRIMARY KEY CLUSTERED ([EMM_Id] ASC),
    CONSTRAINT [FK_EventMeasureMaster_EventTypeMaster] FOREIGN KEY ([EMM_EVM_Id]) REFERENCES [pfp].[EventTypeMaster] ([EVM_Id]),
    CONSTRAINT [FK_EventMeasureMaster_MeasureMaster] FOREIGN KEY ([EMM_MEM_Id]) REFERENCES [pfp].[MeasureMaster] ([MEM_Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is Event Measure master table''s primary key', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventMeasureMaster', @level2type = N'COLUMN', @level2name = N'EMM_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is pfp Measure key', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventMeasureMaster', @level2type = N'COLUMN', @level2name = N'EMM_MEM_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is pfp Event key', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventMeasureMaster', @level2type = N'COLUMN', @level2name = N'EMM_EVM_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Active status for Event Measure', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventMeasureMaster', @level2type = N'COLUMN', @level2name = N'EMM_Active';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record created user''s id', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventMeasureMaster', @level2type = N'COLUMN', @level2name = N'EMM_CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record created date and time', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventMeasureMaster', @level2type = N'COLUMN', @level2name = N'EMM_CreatedOn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record updated/modified user''s id', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventMeasureMaster', @level2type = N'COLUMN', @level2name = N'EMM_UpdatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record updated/modified date and time', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventMeasureMaster', @level2type = N'COLUMN', @level2name = N'EMM_UpdatedOn';

