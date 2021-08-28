use master
create database KitchenBook
use KitchenBook
--drop database KitchenBook

CREATE TABLE [dbo].[Users](
	[ID_User] [int] primary key identity(1,1),
	[Name] [nvarchar](50) NOT NULL,
	[Surame] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Admin] [bit] NOT NULL,
	[Category] [nvarchar](15) NULL,
	[Image] [varbinary](max) NOT NULL
)
--alter table Users add Category nvarchar(15) NULL
CREATE TABLE [dbo].[SecretInfo](
	[ID_User] [int] primary key foreign key(ID_User) references Users(ID_User),
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](10) NOT NULL
)
CREATE TABLE [dbo].[Recipes](
	[ID_recipe] [int] primary key identity(1,1),
	[NameRecipe] [nvarchar](50) NOT NULL,
	[Category] [nvarchar](15) NOT NULL,
	[ID_author] [int] NULL foreign key(ID_author) references Users(ID_User),
	[Description] [nvarchar](max) NOT NULL,
	[Image] [varbinary](max) NOT NULL,
	[Popularity] [int] NOT NULL default(0)
)


--alter table Recipes add Popularity int NOT NULL default(0)


CREATE TABLE [dbo].[UsersRecipes](
	[ID_Like] [int] primary key identity(1,1),
	[ID_User] [int] NOT NULL foreign key(ID_User) references Users(ID_User) ON DELETE CASCADE,
	[ID_recipe] [int] NOT NULL foreign key(ID_recipe) references Recipes(ID_recipe) ON DELETE CASCADE
)
CREATE TABLE [dbo].[Recipe](
	[ID_Ing] [int] primary key identity(1,1),
	[ID_recipe] [int] NOT NULL foreign key(ID_recipe) references Recipes(ID_recipe) ON DELETE CASCADE,
	[Ingredient] [nvarchar](50) NOT NULL,
	[MassIngredient] [nvarchar](20) NOT NULL
)
