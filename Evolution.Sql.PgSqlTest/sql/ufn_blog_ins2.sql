CREATE OR REPLACE FUNCTION blog_ins(
	p_title 		INOUT blog.title%type,
	p_content		blog.content%type,
	p_created_by	blog.created_by%type,
	p_created_on	blog.created_on%type,
	p_updated_by	blog.updated_by%type,
	p_blog_id		OUT blog.id%type
)--RETURNS INT
AS $$
BEGIN
	--select p_title = p_title || '_tial';
	INSERT INTO blog(title, content, created_by, created_on, updated_by)
	VALUES (p_title, p_content, p_created_by, p_created_on, p_updated_by) RETURNING "id" INTO p_blog_id;
END;
$$ LANGUAGE plpgsql;