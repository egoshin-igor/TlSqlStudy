use Test

create table Author(
	Id int identity(1,1) constraint PK_Author primary key,
	Name nvarchar(100)
)

insert into Author
	(Name)
values
	('Паша'),
	('Маша'),
	('Олег')
	
	
select [Id], [Name] from [Author]

update Author
set Name = 'Олег'
where Id = 2

delete from Author
where Id = 3

create table Post(
	Id int identity(1,1) constraint PK_Post primary key,
	Title nvarchar(100),
	Body nvarchar(max),
	AuthorId int constraint FK_Author_Post references Author(Id)
)

insert into Post
values
('Title',
 'Body',
 2 )

select p.Title, p.Body, a.Name AuthorName from Post p
join Author a on p.AuthorId = a.Id

