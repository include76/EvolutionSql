DROP PROCEDURE IF EXISTS usp_user_upd;
DELIMITER //
CREATE PROCEDURE usp_user_upd(
	p_user_id CHAR(36),
	p_first_name VARCHAR(256),
	p_last_name VARCHAR(256),
	p_updated_by VARCHAR(256),
	p_updated_on DATETIME
)
BEGIN
	UPDATE `user`
		SET first_name = p_first_name,
			last_name = p_last_name,
            updated_by = p_updated_by,
            updated_on = p_updated_on
	WHERE user_id = p_user_id;
END//
DELIMITER ;