
create table Category 
(Id int identity primary key,
Name nvarchar(50) not null)

create table Author (
Id int identity primary key,
LastName nvarchar(50), 
FirstName nvarchar(50))

create table Book 
(Id int identity primary key
,Title nvarchar(100) not null
, AuthorId int foreign key references Author,
CategoryId int foreign key references Category,
Pages int,
Cost int,
check(pages < 40000)) 

create proc pAddBook
@Title nvarchar(100),
@AuthorId int,
@CategoryId int,
@Pages int,
@Cost int
as
insert into Book
values(@Title,@AuthorId,@CategoryId,@Pages,@Cost)


exec pAddBook ahhh, 1 ,1, 1, 44



insert into Author
values('john','doe')

insert into Category
values('literature')

drop table book

select * from book

alter proc pUpdBook
@id int,
@Title nvarchar(100),
@AuthorId int,
@CategoryId int,
@Pages int,
@Cost int
as
update Book
set Title = @Title,
AuthorId = @AuthorId,
CategoryId = @CategoryId,
Pages = @Pages,
Cost = @Cost
where id = @id


alter proc pDelBook
@id int
as
Delete from Book
where id = @id

create proc pSelBook
as
select * from Book

select * from Author

create proc pSelBookByID
@id int
as
select * from Book
where id = @id

--begin tran

--begin try 
--	insert into Author
--	values('Тестовый автор Фамилия', 'Тестовый автор Имя')
--	insert into Category
--	values('Тестовая категория')
--	insert into Book
--	values('Словарь', (select id from Author where FirstName = 'Тестовый автор Имя'),  (select id from Category where Name = 'Тестовая категория'), 200, 15000.00),
--	('Книга рецептов',(select id from Author where FirstName = 'Тестовый автор Имя'), (select id from Category where Name = 'Тестовая категория'), 1200, 12000.00),
--	('Уголовный кодекс РК',(select id from Author where FirstName = 'Тестовый автор Имя'), (select id from Category where Name = 'Тестовая категория'), 500000, 20000.00)
--	commit 
--end try

--begin catch
--	rollback
--end  catch

alter proc pAuthorReturnByID
@id int ,
@PagesSum int out,
@BookCount int out,
@BookMaxPrice int out
as

select a.FirstName , a.LastName,c.Name,b.Title
from Author as a join Book as b on b.AuthorId = @id join Category as c on c.Id = b.AuthorId where b.AuthorId = @id 

set @PagesSum = (select sum(pages) from Book where AuthorId = @id)
set @BookCount = (select count(id) from Book where AuthorId = @id)
set @BookMaxPrice = (select max(cost) from Book where AuthorId = @id )
	

Declare @testVar5 int
Declare @testVar6 int
Declare @testVar7 int


exec pAuthorReturnByID 1,@testVar5 output, @testVar6 output,@testVar7 output

