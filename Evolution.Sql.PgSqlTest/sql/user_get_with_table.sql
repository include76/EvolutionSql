CREATE OR REPLACE FUNCTION user_get_with_table(p_user_id UUID)
	RETURNS SETOF "user"
AS $$
BEGIN
	RETURN QUERY SELECT * FROM "user" WHERE user_id = p_user_id;
	--SELECT COUNT(1) INTO p_total_count FROM "user";
END
$$ LANGUAGE plpgsql;