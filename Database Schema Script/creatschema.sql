-- MySQL dump 10.13  Distrib 5.6.19, for Win32 (x86)
--
-- Host: localhost    Database: nbf
-- ------------------------------------------------------
-- Server version	5.6.19

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `bowl`
--

DROP TABLE IF EXISTS `bowl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bowl` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `frameid` int(11) NOT NULL,
  `bowlNumber` int(11) DEFAULT NULL,
  `total` int(11) DEFAULT NULL,
  `isstrike` smallint(6) NOT NULL,
  `isspare` smallint(6) NOT NULL,
  `issplit` smallint(6) NOT NULL,
  `isgutter` smallint(6) NOT NULL,
  `isfoul` smallint(6) NOT NULL,
  `ismanuallymodified` smallint(6) NOT NULL,
  `pins` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `bowl_uk001` (`frameid`,`bowlNumber`),
  KEY `bowl_fk001_idx` (`frameid`),
  CONSTRAINT `bowl_fk001` FOREIGN KEY (`frameid`) REFERENCES `frame` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=40883 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bowlingcenter`
--

DROP TABLE IF EXISTS `bowlingcenter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bowlingcenter` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `uri` varchar(150) DEFAULT NULL,
  `port` int(11) DEFAULT '10011',
  `apiversion` varchar(7) DEFAULT '1.00.00',
  `numberoflanes` int(11) DEFAULT '1',
  `lastsyncdate` datetime DEFAULT NULL,
  `address` varchar(45) DEFAULT NULL,
  `city` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `logo` varchar(100) DEFAULT NULL,
  `phonenumber` varchar(15) DEFAULT NULL,
  `website` varchar(100) DEFAULT NULL,
  `zipcode` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `BowlingCenter_uk001` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `event`
--

DROP TABLE IF EXISTS `event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `eventcode` int(11) NOT NULL,
  `scoresid` int(11) NOT NULL,
  `opendatetime` datetime NOT NULL,
  `closedatetime` datetime DEFAULT NULL,
  `status` enum('NotPlayed','Playing','Played') NOT NULL,
  `openmode` enum('Single','Pair','Group','League','Tournament') NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `event_uk001` (`eventcode`,`scoresid`),
  KEY `event_fk001_idx` (`scoresid`),
  CONSTRAINT `event` FOREIGN KEY (`scoresid`) REFERENCES `scores` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=234 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `favorit`
--

DROP TABLE IF EXISTS `favorit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `favorit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL,
  `favorituserid` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `favorit_uk001` (`userid`,`favorituserid`),
  KEY `favorit_fk002_idx` (`favorituserid`),
  CONSTRAINT `favorit_fk001` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `favorit_fk002` FOREIGN KEY (`favorituserid`) REFERENCES `user` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `frame`
--

DROP TABLE IF EXISTS `frame`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `frame` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `gameid` int(11) NOT NULL,
  `framenumber` int(11) NOT NULL,
  `progressivetotal` int(11) DEFAULT NULL,
  `isconvertedsplit` smallint(6) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `frame_uk001` (`gameid`,`framenumber`),
  CONSTRAINT `frame_fk001` FOREIGN KEY (`gameid`) REFERENCES `game` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=20442 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `game`
--

DROP TABLE IF EXISTS `game`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `game` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `gamecode` int(11) NOT NULL,
  `eventid` int(11) NOT NULL,
  `lanenumber` int(11) NOT NULL,
  `playername` varchar(45) NOT NULL,
  `fullname` varchar(45) NOT NULL,
  `freeentrycode` varchar(45) DEFAULT NULL,
  `playerposition` int(11) NOT NULL,
  `startdatetime` datetime NOT NULL,
  `enddatetime` datetime DEFAULT NULL,
  `gamenumber` int(11) NOT NULL,
  `hdcp` int(11) DEFAULT NULL,
  `total` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `game_uk001` (`eventid`,`gamecode`),
  KEY `game_fk001_idx` (`eventid`),
  CONSTRAINT `game_fk001` FOREIGN KEY (`eventid`) REFERENCES `event` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2276 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `opentime`
--

DROP TABLE IF EXISTS `opentime`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `opentime` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `bowlingcenterid` int(11) NOT NULL,
  `day` varchar(15) DEFAULT NULL,
  `opentime` varchar(5) DEFAULT NULL,
  `closetime` varchar(55) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `opentime_uk1` (`day`,`opentime`,`closetime`,`bowlingcenterid`),
  KEY `opentime_fk001_idx` (`bowlingcenterid`),
  CONSTRAINT `bowlingcenter` FOREIGN KEY (`bowlingcenterid`) REFERENCES `bowlingcenter` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `scores`
--

DROP TABLE IF EXISTS `scores`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `scores` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `bowlingcenterid` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `scores_uk001` (`bowlingcenterid`),
  KEY `scores_fk001_idx` (`bowlingcenterid`),
  CONSTRAINT `scores_fk001` FOREIGN KEY (`bowlingcenterid`) REFERENCES `bowlingcenter` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `roleid` int(11) NOT NULL,
  `logindatetime` double DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `address` varchar(45) DEFAULT NULL,
  `username` varchar(45) DEFAULT NULL,
  `city` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `user_uk002` (`email`),
  UNIQUE KEY `user_uk001` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2014-06-22 15:01:43
