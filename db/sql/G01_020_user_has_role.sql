SET FOREIGN_KEY_CHECKS=0;

USE gamemanager;

DROP TABLE IF EXISTS user_has_role CASCADE;

CREATE TABLE user_has_role
(
	id INT NOT NULL AUTO_INCREMENT COMMENT 'Table primary key',
	user_id INT NOT NULL,
	role VARCHAR(64) NOT NULL,
	active INT NOT NULL DEFAULT 1,
	create_by VARCHAR(32) NOT NULL COMMENT 'Row creation author' DEFAULT "root@init",
	create_time TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) COMMENT 'Row creation time',
	update_by VARCHAR(32) NOT NULL COMMENT 'Row updating author' DEFAULT "root@init",
	update_time TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) COMMENT 'Row updating time',

	CONSTRAINT PK_user_has_role PRIMARY KEY (id ASC)
);

/* indexes */

ALTER TABLE user_has_role 
 ADD INDEX IXFK_user_has_role_user (user_id ASC)
;

/* fks */

ALTER TABLE user_has_role 
 ADD CONSTRAINT FK_user_has_role_user
	FOREIGN KEY (user_id) REFERENCES user (id) ON DELETE Restrict ON UPDATE Restrict
;

CREATE DEFINER=`root`@`localhost` 
TRIGGER `user_has_role_before_insert` BEFORE INSERT ON `user_has_role` 
FOR EACH ROW SET 
	NEW.`create_by` = CURRENT_USER(),
    NEW.`update_by` = CURRENT_USER();

CREATE DEFINER=`root`@`localhost` 
TRIGGER `user_has_role_before_update` BEFORE UPDATE ON `user_has_role` 
FOR EACH ROW SET 
	NEW.`update_by` = CURRENT_USER(),
    NEW.`update_time` = CURRENT_TIMESTAMP(3);

SET FOREIGN_KEY_CHECKS=1; 

