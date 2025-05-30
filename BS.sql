USE [Bookstore]
GO
/****** Object:  Table [dbo].[AgeRatings]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgeRatings](
	[AgeRatingID] [int] IDENTITY(1,1) NOT NULL,
	[Label] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[AgeRatingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[AuthorID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[AuthorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookImage]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookImage](
	[ImageID] [int] IDENTITY(1,1) NOT NULL,
	[ImagePath] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[BookID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[AuthorID] [int] NULL,
	[GenreID] [int] NULL,
	[PublisherID] [int] NULL,
	[YearPublished] [int] NULL,
	[Price] [decimal](10, 2) NULL,
	[Stock] [int] NULL,
	[BookImageID] [int] NULL,
	[SeriesID] [int] NULL,
	[SeriesOrder] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[AgeRatingID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[BookID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookSeries]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookSeries](
	[SeriesID] [int] IDENTITY(1,1) NOT NULL,
	[SeriesName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SeriesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discounts]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discounts](
	[DiscountID] [int] IDENTITY(1,1) NOT NULL,
	[BookID] [int] NULL,
	[DiscountPercent] [int] NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[DiscountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FavoriteBooks]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FavoriteBooks](
	[FavoriteID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
	[DateAdded] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[FavoriteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genres]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genres](
	[GenreID] [int] IDENTITY(1,1) NOT NULL,
	[GenreName] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[GenreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[BookID] [int] NULL,
	[Quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[OrderDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[PaymentDate] [date] NULL,
	[Amount] [decimal](10, 2) NULL,
	[PaymentMethod] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Publishers]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Publishers](
	[PublisherID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[City] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PublisherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[ReviewID] [int] IDENTITY(1,1) NOT NULL,
	[BookID] [int] NULL,
	[UserID] [int] NULL,
	[Rating] [int] NULL,
	[Comment] [nvarchar](300) NULL,
	[ReviewDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[ContactPerson] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[Address] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplies]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplies](
	[SupplyID] [int] IDENTITY(1,1) NOT NULL,
	[BookID] [int] NULL,
	[WarehouseID] [int] NULL,
	[SupplierID] [int] NULL,
	[Quantity] [int] NULL,
	[SupplyDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NULL,
	[Login] [nvarchar](50) NULL,
	[Password] [nvarchar](255) NULL,
	[Email] [nvarchar](100) NULL,
	[BirthDate] [date] NULL,
	[Phone] [nvarchar](20) NULL,
	[RoleID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Warehouses]    Script Date: 28.05.2025 16:51:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Warehouses](
	[WarehouseID] [int] IDENTITY(1,1) NOT NULL,
	[Location] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[WarehouseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AgeRatings] ON 

INSERT [dbo].[AgeRatings] ([AgeRatingID], [Label], [Description]) VALUES (1, N'0+', N'??? ?????????? ???????????')
INSERT [dbo].[AgeRatings] ([AgeRatingID], [Label], [Description]) VALUES (2, N'6+', N'??? ????? ?????? 6 ???')
INSERT [dbo].[AgeRatings] ([AgeRatingID], [Label], [Description]) VALUES (3, N'12+', N'??? ????? ?????? 12 ???')
INSERT [dbo].[AgeRatings] ([AgeRatingID], [Label], [Description]) VALUES (4, N'16+', N'??? ?????????? ?????? 16 ???')
INSERT [dbo].[AgeRatings] ([AgeRatingID], [Label], [Description]) VALUES (5, N'18+', N'?????? ??? ????????')
SET IDENTITY_INSERT [dbo].[AgeRatings] OFF
GO
SET IDENTITY_INSERT [dbo].[Authors] ON 

INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (1, N'Фёдор Достоевский')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (2, N'Джордж Оруэлл')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (3, N'Харуки Мураками')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (1002, N'Ребекка Яррос')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (1003, N'Софи Анри')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (2002, N'Анастасия Гор')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (3002, N'Рита Хоффман')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (3003, N'Анна Джейн')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (3004, N'Майк Омер')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (4004, N'п')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (4005, N'а')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (5004, N'F')
INSERT [dbo].[Authors] ([AuthorID], [FullName]) VALUES (5005, N'й')
SET IDENTITY_INSERT [dbo].[Authors] OFF
GO
SET IDENTITY_INSERT [dbo].[BookImage] ON 

INSERT [dbo].[BookImage] ([ImageID], [ImagePath]) VALUES (1005, N'яррос.jpg')
INSERT [dbo].[BookImage] ([ImageID], [ImagePath]) VALUES (1006, N'РЛ.jpg')
INSERT [dbo].[BookImage] ([ImageID], [ImagePath]) VALUES (1007, N'на.jpg')
INSERT [dbo].[BookImage] ([ImageID], [ImagePath]) VALUES (2005, N'ЖП.jpg')
INSERT [dbo].[BookImage] ([ImageID], [ImagePath]) VALUES (2006, N'YF.jpg')
INSERT [dbo].[BookImage] ([ImageID], [ImagePath]) VALUES (2007, N'НАследник.jpg')
INSERT [dbo].[BookImage] ([ImageID], [ImagePath]) VALUES (2008, N'Аг.jpg')
INSERT [dbo].[BookImage] ([ImageID], [ImagePath]) VALUES (2009, N'1.jpg')
INSERT [dbo].[BookImage] ([ImageID], [ImagePath]) VALUES (2010, N'НЧД.jpg')
SET IDENTITY_INSERT [dbo].[BookImage] OFF
GO
SET IDENTITY_INSERT [dbo].[Books] ON 

INSERT [dbo].[Books] ([BookID], [Title], [AuthorID], [GenreID], [PublisherID], [YearPublished], [Price], [Stock], [BookImageID], [SeriesID], [SeriesOrder], [Description], [AgeRatingID]) VALUES (3005, N'Четвёртое крыло', 1002, 1003, 1009, 2024, CAST(500.00 AS Decimal(10, 2)), 5, 1005, 2, 1, NULL, NULL)
INSERT [dbo].[Books] ([BookID], [Title], [AuthorID], [GenreID], [PublisherID], [YearPublished], [Price], [Stock], [BookImageID], [SeriesID], [SeriesOrder], [Description], [AgeRatingID]) VALUES (3006, N'Наследник Ардена подарочное издание', 1003, 3, 1, 2025, CAST(2999.00 AS Decimal(10, 2)), 3, 2007, 3, NULL, NULL, NULL)
INSERT [dbo].[Books] ([BookID], [Title], [AuthorID], [GenreID], [PublisherID], [YearPublished], [Price], [Stock], [BookImageID], [SeriesID], [SeriesOrder], [Description], [AgeRatingID]) VALUES (3008, N'Железное пламя', 1002, 1003, 1009, 2024, CAST(2399.00 AS Decimal(10, 2)), 5, 2005, 2, 2, N'Продолжение «Четвертого крыла», абсолютного бестселлера, взорвавшего весь мир! Вторая часть саги «Эмпирей» мгновенно возглавила мировые книжные рейтинги NEW YORK TIMES, USA TODAY и AMAZON!
Все знают, из академии Басгиат есть только два выхода – окончить ее или умереть. Никто не ожидал, что Вайолет Сорренгейл сможет пережить первый год. Включая ее саму.
Но как драконы устанавливают собственные законы, так и Вайолет устанавливает свои правила. Тем более что у нее есть определенные преимущества – ум, железная воля, преданные друзья и любовь Ксейдена Риорсона.
Однако одной решимости выжить становится мало. Ведь Вайолет знает столько тайн, что голова идет кругом! Но секреты – плохой рычаг давления. Они умирают вместе с теми, кто их хранит. И даже драконьего огня может оказаться недостаточно, чтобы спалить всех ее врагов. Что ж… Добро пожаловать в революцию!
«Железное пламя» – огненное продолжение истории, которую читаешь на одном дыхании, и не замечаешь, как наступает утро.
Роман издается в специальном оформлении: обложка с эффектом металлического блеска, стильный черный обрез. Эксклюзивно от издательства «Кислород»!', 4)
INSERT [dbo].[Books] ([BookID], [Title], [AuthorID], [GenreID], [PublisherID], [YearPublished], [Price], [Stock], [BookImageID], [SeriesID], [SeriesOrder], [Description], [AgeRatingID]) VALUES (3009, N'Рубиновый лес подарочное издание', 2002, 3, 1, 2025, CAST(1333.00 AS Decimal(10, 2)), 5, 2008, 5, NULL, N'', 4)
INSERT [dbo].[Books] ([BookID], [Title], [AuthorID], [GenreID], [PublisherID], [YearPublished], [Price], [Stock], [BookImageID], [SeriesID], [SeriesOrder], [Description], [AgeRatingID]) VALUES (3010, N'Мрачный взвод подарочное издание', 3002, 3, 1, 2023, CAST(2333.00 AS Decimal(10, 2)), 6, 2009, 1005, NULL, NULL, NULL)
INSERT [dbo].[Books] ([BookID], [Title], [AuthorID], [GenreID], [PublisherID], [YearPublished], [Price], [Stock], [BookImageID], [SeriesID], [SeriesOrder], [Description], [AgeRatingID]) VALUES (3011, N'Наследница чёрного дракона подарочная версия', 3003, 3, 1, 2024, CAST(1500.00 AS Decimal(10, 2)), 5, 2010, 4, 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Books] OFF
GO
SET IDENTITY_INSERT [dbo].[BookSeries] ON 

INSERT [dbo].[BookSeries] ([SeriesID], [SeriesName]) VALUES (1, N'Эмперией')
INSERT [dbo].[BookSeries] ([SeriesID], [SeriesName]) VALUES (2, N'Эмпирей')
INSERT [dbo].[BookSeries] ([SeriesID], [SeriesName]) VALUES (3, N'Игры королей')
INSERT [dbo].[BookSeries] ([SeriesID], [SeriesName]) VALUES (4, N'Нежеланная невеста')
INSERT [dbo].[BookSeries] ([SeriesID], [SeriesName]) VALUES (5, N'Рубиновый лес')
INSERT [dbo].[BookSeries] ([SeriesID], [SeriesName]) VALUES (1005, N'Мрачный взвод')
INSERT [dbo].[BookSeries] ([SeriesID], [SeriesName]) VALUES (2005, N'п')
INSERT [dbo].[BookSeries] ([SeriesID], [SeriesName]) VALUES (2006, N'пп')
INSERT [dbo].[BookSeries] ([SeriesID], [SeriesName]) VALUES (2007, N'пПИпеу')
INSERT [dbo].[BookSeries] ([SeriesID], [SeriesName]) VALUES (2008, N'АП')
SET IDENTITY_INSERT [dbo].[BookSeries] OFF
GO
SET IDENTITY_INSERT [dbo].[Discounts] ON 

INSERT [dbo].[Discounts] ([DiscountID], [BookID], [DiscountPercent], [StartDate], [EndDate]) VALUES (1, 3008, 69, CAST(N'2025-05-02' AS Date), CAST(N'2025-05-31' AS Date))
SET IDENTITY_INSERT [dbo].[Discounts] OFF
GO
SET IDENTITY_INSERT [dbo].[Genres] ON 

INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (1, N'Классика')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (2, N'Фантастика')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (3, N'Роман')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (4, N'Детектив')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (1002, N'Триллер')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (1003, N'Фэнтези')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (1004, N'Приключения')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (1005, N'Исторический роман')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (1006, N'Ужасы')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (1007, N'Азиатское и славянское фэнтези')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (1008, N'non-fiction ')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (1009, N'young adult.')
INSERT [dbo].[Genres] ([GenreID], [GenreName]) VALUES (1010, N'Научная литература')
SET IDENTITY_INSERT [dbo].[Genres] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 

INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [BookID], [Quantity]) VALUES (1006, 1003, 3006, 1)
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [BookID], [Quantity]) VALUES (1007, 1003, 3009, 1)
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [BookID], [Quantity]) VALUES (1008, 1003, 3010, 1)
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [BookID], [Quantity]) VALUES (1009, 1003, 3008, 1)
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [BookID], [Quantity]) VALUES (1010, 1004, 3008, 1)
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderDate]) VALUES (2, 2, CAST(N'2025-05-02' AS Date))
INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderDate]) VALUES (1003, 1, CAST(N'2025-05-22' AS Date))
INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderDate]) VALUES (1004, 1, CAST(N'2025-05-27' AS Date))
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Publishers] ON 

INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (1, N'Эксмо', N'Москва')
INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (2, N'Азбука', N'Санкт-Петербург')
INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (1002, N'АСТ', N'Москва')
INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (1003, N'Рипол Классик', N'Москва')
INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (1004, N'Манн, Иванов и Фербер', N'Москва')
INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (1005, N'TrendBooks', N'Москва')
INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (1006, N'Самокат', N'Москва')
INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (1007, N'Freedom', N'Москва')
INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (1008, N'Popcornbooks', N'Москва')
INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (1009, N'O2', N'Санкт-Петербург')
INSERT [dbo].[Publishers] ([PublisherID], [Name], [City]) VALUES (1010, N'Клевер-Медиа-Групп', N'Москва')
SET IDENTITY_INSERT [dbo].[Publishers] OFF
GO
SET IDENTITY_INSERT [dbo].[Reviews] ON 

INSERT [dbo].[Reviews] ([ReviewID], [BookID], [UserID], [Rating], [Comment], [ReviewDate]) VALUES (3, 3008, 1, 5, N'пваопвапвап', CAST(N'2025-05-22' AS Date))
SET IDENTITY_INSERT [dbo].[Reviews] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'admin')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'user')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Suppliers] ON 

INSERT [dbo].[Suppliers] ([SupplierID], [Name], [ContactPerson], [Phone], [Email], [Address]) VALUES (1, N'ООО Книготорг', N'Светлана Николаева', N'89112225544', N'supply@books.ru', N'Москва, ул. Тверская, 22')
SET IDENTITY_INSERT [dbo].[Suppliers] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [FullName], [Login], [Password], [Email], [BirthDate], [Phone], [RoleID]) VALUES (1, N'Admin admin', N'admin', N'admin', N'admin@example.com', CAST(N'1985-01-01' AS Date), N'+79000000000', 1)
INSERT [dbo].[Users] ([UserID], [FullName], [Login], [Password], [Email], [BirthDate], [Phone], [RoleID]) VALUES (2, N'Ivan Ivanov', N'ivan', N'ivanpass', N'ivan@example.com', CAST(N'1995-05-10' AS Date), N'+79000000001', 2)
INSERT [dbo].[Users] ([UserID], [FullName], [Login], [Password], [Email], [BirthDate], [Phone], [RoleID]) VALUES (3, N'Maria Smirnova', N'maria', N'mariapass', N'maria@example.com', CAST(N'1998-11-25' AS Date), N'+79000000002', 2)
INSERT [dbo].[Users] ([UserID], [FullName], [Login], [Password], [Email], [BirthDate], [Phone], [RoleID]) VALUES (4, N'Petr Sidorov', N'petr', N'petrpass', N'petr@example.com', CAST(N'1992-08-15' AS Date), N'+79000000003', 2)
INSERT [dbo].[Users] ([UserID], [FullName], [Login], [Password], [Email], [BirthDate], [Phone], [RoleID]) VALUES (5, N'Анна', N'anna', N'Anna12345', N'anna@gmail.com', CAST(N'2025-05-01' AS Date), N'+9817502755', 2)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Warehouses] ON 

INSERT [dbo].[Warehouses] ([WarehouseID], [Location]) VALUES (1, N'Склад №1, Москва')
SET IDENTITY_INSERT [dbo].[Warehouses] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Roles__8A2B61602DAE4BDF]    Script Date: 28.05.2025 16:51:04 ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__5E55825B5684BC0B]    Script Date: 28.05.2025 16:51:04 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FavoriteBooks] ADD  DEFAULT (getdate()) FOR [DateAdded]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD FOREIGN KEY([AuthorID])
REFERENCES [dbo].[Authors] ([AuthorID])
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD FOREIGN KEY([GenreID])
REFERENCES [dbo].[Genres] ([GenreID])
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD FOREIGN KEY([PublisherID])
REFERENCES [dbo].[Publishers] ([PublisherID])
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_AgeRatings] FOREIGN KEY([AgeRatingID])
REFERENCES [dbo].[AgeRatings] ([AgeRatingID])
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_Books_AgeRatings]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_BookImage] FOREIGN KEY([BookImageID])
REFERENCES [dbo].[BookImage] ([ImageID])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_Books_BookImage]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_BookSeries] FOREIGN KEY([SeriesID])
REFERENCES [dbo].[BookSeries] ([SeriesID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_Books_BookSeries]
GO
ALTER TABLE [dbo].[Discounts]  WITH CHECK ADD FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
GO
ALTER TABLE [dbo].[FavoriteBooks]  WITH CHECK ADD FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
GO
ALTER TABLE [dbo].[FavoriteBooks]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [FK_Reviews_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_Reviews_Users]
GO
ALTER TABLE [dbo].[Supplies]  WITH CHECK ADD FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
GO
ALTER TABLE [dbo].[Supplies]  WITH CHECK ADD FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Suppliers] ([SupplierID])
GO
ALTER TABLE [dbo].[Supplies]  WITH CHECK ADD FOREIGN KEY([WarehouseID])
REFERENCES [dbo].[Warehouses] ([WarehouseID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
