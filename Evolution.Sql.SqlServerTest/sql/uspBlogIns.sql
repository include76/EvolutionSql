create procedure uspBlogIns(@Title nvarchar(1000),
	@content nvarchar(max),
	@CreatedBy varchar(256),
	@CreatedOn datetime2
)
as
begin
	insert into Blog values(@Title, @content, @CreatedBy, @CreatedOn)
	select SCOPE_IDENTITY()
end