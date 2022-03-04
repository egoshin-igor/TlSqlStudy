use Blogs

create table Author(
	Id int identity(1,1) constraint PK_Author primary key,
	Name nvarchar(100)
)

select * from Author

insert into Author
	(Name)
values
	('Иван'),
	('Маша'),
	('Паша')

update Author
set Name = 'Олег'
where Id = 2

delete from Author
where Id = 2


create table Post(
	Id int identity(1,1) constraint PK_Post primary key,
	Title nvarchar(100),
	Body nvarchar(max),
	AuthorId int constraint FK_Post_Author references Author(Id)
)

select * from Post
select * from Author

insert into Post
values
('Заголовок 1', 'Тело новости', 3),
('Заголовок 2', 'Тело новости 2', 3),
('Заголовок 3', 'Тело новости 3', 1)

select p.Title, p.Body, a.Name as AuthorName from Post as p
join Author a on a.Id = p.AuthorId
