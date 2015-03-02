CREATE TABLE [dbo].[Artists] (
    [id]         INT           IDENTITY (1, 1) NOT NULL,
    [artistName] VARCHAR (250) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    UNIQUE NONCLUSTERED ([artistName] ASC)
);
CREATE TABLE [dbo].[HashTags] (
    [id]      INT           IDENTITY (1, 1) NOT NULL,
    [hashtag] VARCHAR (250) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    UNIQUE NONCLUSTERED ([hashtag] ASC)
);

CREATE TABLE [dbo].[EventManagers] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [username]         VARCHAR (250) NOT NULL,
    [password]         VARCHAR (250) NOT NULL,
    [email]            VARCHAR (250) NOT NULL,
    [facebookPage]     VARCHAR (30)  NULL,
    [facebookPassword] VARCHAR (20)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    UNIQUE NONCLUSTERED ([username] ASC)
);

CREATE TABLE [dbo].[Events] (
    [id]               INT          IDENTITY (1, 1) NOT NULL,
    [EventName]        VARCHAR (50) NOT NULL,
    [EventManagerId]   INT          NOT NULL,
    [date]             DATETIME     NOT NULL,
    [duration]         FLOAT (53)   NULL,
    [description]      TEXT         NULL,
    [price]            FLOAT (53)   NOT NULL,
    [discount]         FLOAT (53)   NULL,
    [maxNumOfTickets]  INT          DEFAULT ((0)) NOT NULL,
    [soldTickets]      INT          DEFAULT ((0)) NOT NULL,
    [availableTickets] INT          NULL,
    [longitude]        FLOAT (53)   DEFAULT ((0)) NOT NULL,
    [latitude]         FLOAT (53)   DEFAULT ((0)) NOT NULL,
    [address] NCHAR(250) NOT NULL, 
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [AK_Events_manager] UNIQUE NONCLUSTERED ([EventManagerId] ASC, [EventName] ASC),
    CONSTRAINT [FK_EventManager] FOREIGN KEY ([EventManagerId]) REFERENCES [dbo].[EventManagers] ([id])
);

CREATE TABLE [dbo].[hashTagsOfEvents] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [eventId] INT NOT NULL,
    [tagId]   INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_User_Tag1] UNIQUE NONCLUSTERED ([eventId] ASC, [tagId] ASC),
    CONSTRAINT [FK_eventsTags_Events] FOREIGN KEY ([eventId]) REFERENCES [dbo].[Events] ([id]),
    CONSTRAINT [FK_UserTags_tags1] FOREIGN KEY ([tagId]) REFERENCES [dbo].[HashTags] ([id])
);

CREATE TABLE [dbo].[Users] (
    [id]               INT          IDENTITY (1, 1) NOT NULL,
    [firstName]        VARCHAR (20) NULL,
    [lastName]         VARCHAR (20) NULL,
    [birthDate]        DATETIME     NULL,
    [email]            VARCHAR (30) NULL,
    [username]         VARCHAR (30) NULL,
    [password]         VARCHAR (20) NULL,
    [hasAllerts]       BIT          NULL,
    [facebookUser]     VARCHAR (20) NULL,
    [facebookPassword] VARCHAR (20) NULL,
    [longitude]        FLOAT (53)   NULL,
    [latitude]         FLOAT (53)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    UNIQUE NONCLUSTERED ([username] ASC)
);

CREATE TABLE [dbo].[hashTagsOfUsers] (
    [Id]     INT IDENTITY (1, 1) NOT NULL,
    [userId] INT NOT NULL,
    [tagId]  INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_User_Tag] UNIQUE NONCLUSTERED ([userId] ASC, [tagId] ASC),
    CONSTRAINT [FK_UserTags_user] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users] ([id]),
    CONSTRAINT [FK_UserTags_tags] FOREIGN KEY ([tagId]) REFERENCES [dbo].[HashTags] ([id])
);


CREATE TABLE [dbo].[ArtistsInEvents] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [eventId]  INT NOT NULL,
    [artistId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_event_artist] UNIQUE NONCLUSTERED ([artistId] ASC, [eventId] ASC),
    CONSTRAINT [FK_ea_artist] FOREIGN KEY ([artistId]) REFERENCES [dbo].[Artists] ([id]),
    CONSTRAINT [FK_ea_events] FOREIGN KEY ([eventId]) REFERENCES [dbo].[Events] ([id])
);
