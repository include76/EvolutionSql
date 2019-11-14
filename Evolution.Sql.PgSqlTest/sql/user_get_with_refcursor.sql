CREATE OR REPLACE FUNCTION user_get_with_refcursor(p_user_id UUID)
	RETURNS refcursor
AS $$
DECLARE
    ref refcursor;
BEGIN
	OPEN ref FOR SELECT * FROM "user" WHERE user_id = p_user_id;
	RETURN ref;
	--SELECT COUNT(1) INTO p_total_count FROM "user";
END
$$ 	LANGUAGE plpgsql;