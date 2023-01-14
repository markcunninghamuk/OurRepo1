CREATE TABLE [dbo].[Sheep]
(
    [Id] [uniqueidentifier] NOT NULL,
    [FirstName] [varchar](100) NOT NULL,
    [LastName] [varchar](200) NOT NULL,
    [ColourType] int NOT NULL,
    [VisualAttributesAsCsv] [varchar](200) NULL,
    [ImageUrl] [varchar](200) NULL,
    [MedicalHistory] [varchar](2000) NULL,
    [ChangedByUserId] [uniqueidentifier] NOT NULL,
    [AuditId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Sheep] ADD  CONSTRAINT [PK_SheepId] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)


--SELECT TOP (1000) 'INSERT INTO Sheep (Id, FirstName, LastName, ColourType, VisualAttributesAsCsv, ImageUrl, MedicalHistory, ChangedByUserId, AuditId) values ('''\
-- + convert(nvarchar(36), Id) + ''',''' + FirstName + ''',''' + LastName + ''',' + convert(nvarchar(1), ColourType) + ',''' + VisualAttributesAsCsv + ''',''' +\
--     ImageUrl + ''',''' + MedicalHistory + ''',''' + convert(nvarchar(36),ChangedByUserId) + ''',''' + convert(nvarchar(36), AuditId) + ''')'\
-- FROM [CdpDev].[dbo].[Sheep]\


INSERT INTO Sheep (Id, FirstName, LastName, ColourType, VisualAttributesAsCsv, ImageUrl, MedicalHistory, ChangedByUserId, AuditId) values ('7AB6E452-BFF1-44AB-A41B-37955AAEEBF6','Shaggy','Sheep',1,'some-white-face|long-haired','https://upload.wikimedia.org/wikipedia/commons/thumb/2/29/Cotswold_Sheep_%28cropped%29.JPG/1200px-Cotswold_Sheep_%28cropped%29.JPG','Needs a haircut', '00000000-0000-0000-0000-000000000000','E3911965-C7DE-4F22-BEAD-E470AA2AB30C');
INSERT INTO Sheep (Id, FirstName, LastName, ColourType, VisualAttributesAsCsv, ImageUrl, MedicalHistory, ChangedByUserId, AuditId) values ('4EEC8083-D5D5-4261-A33D-3A6963AFEF69','Vanilla','Sheep',1,'some-white-face|branding-mark','https://m.media-amazon.com/images/I/81BA0EEWNoL._AC_SX679_.jpg','Healthy','00000000-0000-0000-0000-000000000000','0D926864-85B7-4E2D-B62E-C59D16C87A8A');
INSERT INTO Sheep (Id, FirstName, LastName, ColourType, VisualAttributesAsCsv, ImageUrl, MedicalHistory, ChangedByUserId, AuditId) values ('4F2A1A5C-2693-4E31-BEBC-515B45887CC8','Jumping','Lamb',3,'some-white-face|some-black-face','https://d1qvp5bygkarui.cloudfront.net/uploads/2018/04/dinky-the-sheep.jpg','Needs to not jump so much','00000000-0000-0000-0000-000000000000','8143F404-AB98-4878-BE97-763529AB4826');
INSERT INTO Sheep (Id, FirstName, LastName, ColourType, VisualAttributesAsCsv, ImageUrl, MedicalHistory, ChangedByUserId, AuditId) values ('1908DB08-0D2A-403A-9D31-9613997260F2','Cartoon','Sheep',1,'some-white-face','https://img.freepik.com/free-vector/cute-sheep-animal-cartoon-sticker_1308-76259.jpg?w=2000','Inanimate','00000000-0000-0000-0000-000000000000','AC02CB17-A206-4169-AE0D-B39D47DF09A8');
INSERT INTO Sheep (Id, FirstName, LastName, ColourType, VisualAttributesAsCsv, ImageUrl, MedicalHistory, ChangedByUserId, AuditId) values ('97F2C775-5A02-4FE7-AD06-C920287054A2','Shaun','The Sheep',3,'some-black-face','https://www.britishironworkcentre.co.uk/shop/media/catalog/product/cache/f3a4c85e22551b1880e2e84d85b69ee0/s/h/shaun_the_sheep_garden_ornament.jpg','Inanimate','00000000-0000-0000-0000-000000000000','23E47986-AB3B-4DB3-AA47-F21AA8847C76');
