SET FOREIGN_KEY_CHECKS=0;

USE gamemanager;
 
DROP TABLE IF EXISTS game_media CASCADE;

CREATE TABLE game_media
(
	id INT NOT NULL AUTO_INCREMENT COMMENT 'Table primary key',
	borrower_id INT NULL,
	title VARCHAR(100) NOT NULL,
	year INT(4) NOT NULL,
	platform INT NOT NULL DEFAULT 0 COMMENT '0 - Not defined, 1 - PC, 2 - SNES, 3 - PS',
	media_type INT NOT NULL DEFAULT 0 COMMENT '0 - Not defined, 1 - CD, 2 - Cartridge',
	active INT NOT NULL DEFAULT 1,
	create_by VARCHAR(32) NOT NULL COMMENT 'Row creation author' DEFAULT "root@init",
	create_time TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) COMMENT 'Row creation time',
	update_by VARCHAR(32) NOT NULL COMMENT 'Row updating author' DEFAULT "root@init",
	update_time TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) COMMENT 'Row updating time',

	CONSTRAINT PK_game_media PRIMARY KEY (id ASC)
);

/* indexes */

ALTER TABLE game_media 
 ADD INDEX IXFK_game_media_borrower (borrower_id ASC)
;

/* fks */

ALTER TABLE game_media 
 ADD CONSTRAINT FK_game_media_friend
	FOREIGN KEY (borrower_id) REFERENCES friend (id) ON DELETE Restrict ON UPDATE Restrict
;

/* triggers */

CREATE DEFINER=`root`@`localhost` 
TRIGGER `game_media_before_insert` BEFORE INSERT ON `game_media` 
FOR EACH ROW SET 
	NEW.`create_by` = CURRENT_USER(),
    NEW.`update_by` = CURRENT_USER()
;

CREATE DEFINER=`root`@`localhost` 
TRIGGER `game_media_before_update` BEFORE UPDATE ON `game_media` 
FOR EACH ROW SET 
	NEW.`update_by` = CURRENT_USER(),
    NEW.`update_time` = CURRENT_TIMESTAMP(3)
;

SET FOREIGN_KEY_CHECKS=1; 

