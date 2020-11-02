SET FOREIGN_KEY_CHECKS=0;

USE gamemanager;
 
DROP TABLE IF EXISTS friend CASCADE;

CREATE TABLE friend
(
	id INT NOT NULL AUTO_INCREMENT COMMENT 'Table primary key',
	name VARCHAR(100) NOT NULL,
	email VARCHAR(100) NOT NULL,
	telephone VARCHAR(20) NOT NULL,
	active INT NOT NULL DEFAULT 1,
	create_by VARCHAR(32) NOT NULL COMMENT 'Row creation author' DEFAULT "root@init",
	create_time TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) COMMENT 'Row creation time',
	update_by VARCHAR(32) NOT NULL COMMENT 'Row updating author' DEFAULT "root@init",
	update_time TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) COMMENT 'Row updating time',

	CONSTRAINT PK_friend PRIMARY KEY (id ASC)
);

/* triggers */

CREATE DEFINER=`root`@`localhost` 
TRIGGER `friend_before_insert` BEFORE INSERT ON `friend` 
FOR EACH ROW SET 
	NEW.`create_by` = CURRENT_USER(),
    NEW.`update_by` = CURRENT_USER()
;

CREATE DEFINER=`root`@`localhost` 
TRIGGER `friend_before_update` BEFORE UPDATE ON `friend` 
FOR EACH ROW SET 
	NEW.`update_by` = CURRENT_USER(),
    NEW.`update_time` = CURRENT_TIMESTAMP(3)
;

SET FOREIGN_KEY_CHECKS=1; 

