
drop database blog ;
create database blog;
use blog;

create table `user`(
	UserId char(36) not null,
	FirstName varchar(256) not null,
	LastName varchar(256) not null,
	CreatedOn datetime,
	UpdatedOn datetime,
	constraint pk_user primary key(UserId)
)engine=InnoDB default charset = utf8;

create table tag(
	Id int not null auto_increment,
	`Name` varchar(256) not null,
	Description varchar(2000),
	constraint pk_tag primary key(Id)
)engine=InnoDB default charset = utf8;

create table blog(
	Id int not null auto_increment,
	Title varchar(1000) not null,
	Content text not null,
	CreatedBy char(36) not null,
	CreatedOn datetime not null,
	UpdatedOn datetime null,
	constraint pk_blog primary key(Id),
	constraint fk_blog_user foreign key(CreatedBy) references `User`(UserId)
)engine=InnoDB default charset = utf8;

create table post(
	Id int not null auto_increment,
	Content text not null,
	CreatedBy char(36) not null,
	CreatedOn datetime not null,
	UpdatedOn datetime not null,
	PostId int not null,
	constraint pk_post primary key(Id),
	constraint fk_post_user foreign key(CreatedBy) references `user`(UserId),
	constraint fk_post_blog foreign key(PostId) references blog(Id)
)engine=InnoDB default charset = utf8;
