CREATE OR REPLACE FUNCTION blog_ins(
	p_title 		blog.title%type,
	p_content		blog.content%type,
	p_created_by	blog.created_by%type,
	p_created_on	blog.created_on%type
	--p_blog_id		OUT blog.id%type
)RETURNS INT
AS $$
--BEGIN
	INSERT INTO blog(title, content, created_by, created_on)
	VALUES (p_title, p_content, p_created_by, p_created_on) RETURNING "id";
--END;
$$ LANGUAGE sql;