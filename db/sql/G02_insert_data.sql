-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               8.0.22 - MySQL Community Server - GPL
-- Server OS:                    Linux
-- HeidiSQL Version:             11.0.0.6113
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- Dumping data for table gamemanager.user: ~1 rows (approximately)
/*!40000 ALTER TABLE `user` DISABLE KEYS */;

INSERT INTO `user` (`id`, `name`, `login`, `password`, `salt`, `active`, `create_by`, `create_time`, `update_by`, `update_time`) VALUES
	(1, 'Danilo', 'admin', 'MheSonAWe3inASHHE67ExDvS3ZcCDQWhpk1HCO8O65WEc4VbFKKrCQbwYjsmzrlxAlj31xndriTdtFyBxaDjMw==', 'aRUBXBmY0k628Uxa0cEw1DXhcxE2e6ziT7joI27KuPBLMGgEffMihWMHPq2zK0cW1bYwdPJga8eJNWb7bv6+sOnS6FO03DI63B14+HkbBO096c4mLCSd6Hu7zRZPJMtysAdctseD/jFCQXLZZhouf6iPFyja28egPfdI6qLkeA4=', 0, 'root@localhost', '2020-10-31 21:15:07.656', 'root@localhost', '2020-10-31 21:15:07.656');

INSERT INTO `gamemanager`.`user_has_role` (`user_id`, `role`) VALUES ('1', 'ADMIN');

INSERT INTO `gamemanager`.`game_media` 
(`title`, `year`, `platform`, `media_type`) VALUES 
('Super Mario World', '1995', '2', '2'),
('Crash Bandicoot', '1996', '3', '1'),
('Command & Conquer', '1995', '1', '1'),
('Age of Empires', '1997', '1', '1'),
('Sim City', '1989', '2', '2')
;

INSERT INTO `gamemanager`.`friend` 
(`name`, `email`, `telephone`) VALUES 
('Finn', 'finn@ooo.com', '991882771'),
('Jake', 'jake@ooo.com', '91992819'),
('Bmo', 'bmo@ooo.com', '99288172'),
('Ice King', 'ice@ooo.com', '9199242'),
('Marceline', 'marceline@ooo.com', '93881772')
;


/*!40000 ALTER TABLE `user` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
