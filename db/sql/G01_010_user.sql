SET FOREIGN_KEY_CHECKS=0;
SET character_set_client = utf8;
SET character_set_connection = utf8;
SET character_set_results = utf8;
SET collation_connection = utf8_general_ci;

USE gamemanager;

DROP TABLE IF EXISTS user CASCADE;

CREATE TABLE user
(
	id INT NOT NULL AUTO_INCREMENT COMMENT 'Table primary key',
	name VARCHAR(100) NOT NULL,
	login VARCHAR(64) NOT NULL,
	password VARCHAR(200) NOT NULL,
	salt VARCHAR(200) NOT NULL,
	active INT NOT NULL DEFAULT 1,
	create_by VARCHAR(32) NOT NULL COMMENT 'Row creation author' DEFAULT "root@init",
	create_time TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) COMMENT 'Row creation time',
	update_by VARCHAR(32) NOT NULL COMMENT 'Row updating author' DEFAULT "root@init",
	update_time TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) COMMENT 'Row updating time',

	CONSTRAINT PK_user PRIMARY KEY (id ASC)
);

CREATE DEFINER=`root`@`localhost` 
TRIGGER `user_before_insert` BEFORE INSERT ON `user` 
FOR EACH ROW SET 
	NEW.`create_by` = CURRENT_USER(),
    NEW.`update_by` = CURRENT_USER();

CREATE DEFINER=`root`@`localhost` 
TRIGGER `user_before_update` BEFORE UPDATE ON `user` 
FOR EACH ROW SET 
	NEW.`update_by` = CURRENT_USER(),
    NEW.`update_time` = CURRENT_TIMESTAMP(3);

SET FOREIGN_KEY_CHECKS=1; 

