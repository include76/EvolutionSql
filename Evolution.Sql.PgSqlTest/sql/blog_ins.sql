CREATE OR REPLACE FUNCTION blog_ins(p_title type%blog.title,
	p_content type%blog."content",
	p_created_by type%blog.created_by,
	p_created_on type%blog.created_on,
	p_updated_by type%blog.updated_by,
	p_updated_on type%blog_updated_on
)
	RETURN INT
	LANGUAGE plpgsql
AS $$
BEGIN
	INSERT INTO blog
	VALUES(p_title, p_content, p_created_by, p_created_on) RETURNNING id;
END
$$;