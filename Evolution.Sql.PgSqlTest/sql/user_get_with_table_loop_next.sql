CREATE OR REPLACE FUNCTION user_get_with_table_loop_next(p_user_id UUID)
	--RETURNS SETOF "user"
	returns table(
		user_id	uuid,
		first_name CHARACTER VARYING(256),
		last_name VARCHAR(256),
		created_by VARCHAR(256),
		created_on TIMESTAMP,
		updated_by CHARACTER VARYING(256),
		updated_on TIMESTAMP
	)
AS $$
DECLARE var_r RECORD;
BEGIN	
	FOR var_r IN(SELECT * FROM "user" u WHERE u.user_id = p_user_id)
	LOOP
		user_id = var_r.user_id;
		first_name = var_r.first_name;
		last_name = var_r.last_name;
		created_by = var_r.created_by;
		created_on = var_r.created_on;
		updated_by = var_r.updated_by;
		updated_on = var_r.updated_on;
		RETURN NEXT;
	END LOOP;
	--SELECT COUNT(1) INTO p_total_count FROM "user";
END;
$$ LANGUAGE plpgsql;