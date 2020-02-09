CREATE TABLE [pfp].[CalendarMaster] (
    [CAL_Id]        INT          IDENTITY (1, 1) NOT NULL,
    [CAL_Year]      INT          NOT NULL,
    [CAL_Month]     INT          NOT NULL,
    [CAL_MonthText] VARCHAR (3)  NOT NULL,
    [CAL_Active]    BIT          CONSTRAINT [DF__CalendarM__CAL_A__68487DD7] DEFAULT ((1)) NOT NULL,
    [CAL_OrderBy]   INT          NOT NULL,
    [CAL_CreatedBy] VARCHAR (50) NOT NULL,
    [CAL_CreatedOn] DATETIME     CONSTRAINT [DF__CalendarM__CAL_C__693CA210] DEFAULT (getdate()) NOT NULL,
    [CAL_UpdatedBy] VARCHAR (50) NOT NULL,
    [CAL_UpdatedOn] DATETIME     NOT NULL,
    CONSTRAINT [PK_CAL_Id_CalendarMaster] PRIMARY KEY CLUSTERED ([CAL_Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is Calendar master table''s primary key', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'CalendarMaster', @level2type = N'COLUMN', @level2name = N'CAL_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is pfp Year value', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'CalendarMaster', @level2type = N'COLUMN', @level2name = N'CAL_Year';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is pfp month value', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'CalendarMaster', @level2type = N'COLUMN', @level2name = N'CAL_Month';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is pfp month value', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'CalendarMaster', @level2type = N'COLUMN', @level2name = N'CAL_MonthText';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Active status for Calendar', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'CalendarMaster', @level2type = N'COLUMN', @level2name = N'CAL_Active';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record created user''s id', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'CalendarMaster', @level2type = N'COLUMN', @level2name = N'CAL_CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record created date and time', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'CalendarMaster', @level2type = N'COLUMN', @level2name = N'CAL_CreatedOn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record updated/modified user''s id', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'CalendarMaster', @level2type = N'COLUMN', @level2name = N'CAL_UpdatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is record updated/modified date and time', @level0type = N'SCHEMA', @level0name = N'pfp', @level1type = N'TABLE', @level1name = N'CalendarMaster', @level2type = N'COLUMN', @level2name = N'CAL_UpdatedOn';

