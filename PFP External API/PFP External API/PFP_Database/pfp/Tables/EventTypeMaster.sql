CREATE TABLE [pfp].[EventTypeMaster] (
    [EVM_Id]        INT           IDENTITY (1, 1) NOT NULL,
    [EVM_EventType] VARCHAR (200) NOT NULL,
    [EVM_Active]    BIT           CONSTRAINT [DF__EventMast__EVM_A__02FC7413] DEFAULT ((1)) NOT NULL,
    [EVM_CreatedBy] VARCHAR (50)  NOT NULL,
    [EVM_CreatedOn] DATETIME      CONSTRAINT [DF__EventMast__EVM_C__03F0984C] DEFAULT (getdate()) NOT NULL,
    [EVM_UpdatedBy] VARCHAR (50)  NOT NULL,
    [EVM_UpdatedOn] DATETIME      NOT NULL,
    CONSTRAINT [PK_EVM_Id_EventMaster] PRIMARY KEY CLUSTERED ([EVM_Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is Event master table''s primary key', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventTypeMaster', @level2type = N'COLUMN', @level2name = N'EVM_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is pfp Event value', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventTypeMaster', @level2type = N'COLUMN', @level2name = N'EVM_EventType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Active status for Event', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventTypeMaster', @level2type = N'COLUMN', @level2name = N'EVM_Active';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record created user''s id', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventTypeMaster', @level2type = N'COLUMN', @level2name = N'EVM_CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record created date and time', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventTypeMaster', @level2type = N'COLUMN', @level2name = N'EVM_CreatedOn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record updated/modified user''s id', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventTypeMaster', @level2type = N'COLUMN', @level2name = N'EVM_UpdatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record updated/modified date and time', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'EventTypeMaster', @level2type = N'COLUMN', @level2name = N'EVM_UpdatedOn';

