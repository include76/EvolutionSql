CREATE OR REPLACE FUNCTION user_get(p_user_id INT, OUT p_total_count)
	RETURN INT
	LANGUAGE plpgsql
AS $$
BEGIN
	SELECT * FROM "user" WHERE user_id = p_user_id;
	SELECT COUNT(1) INTO p_total_count;
END
$$;