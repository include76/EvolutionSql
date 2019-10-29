DROP PROCEDURE IF EXISTS usp_user_get;
DELIMITER //
CREATE PROCEDURE usp_user_get(
	IN pUserId CHAR(36),
	OUT totalCount INT
)
BEGIN
	SELECT * FROM `user` 
    WHERE user_id = pUserId;
	SELECT count(1) into totalCount FROM `user`;
END//
DELIMITER ;