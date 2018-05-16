/*
Navicat MySQL Data Transfer

Source Server         : MySQL-Sun
Source Server Version : 50721
Source Host           : 47.75.54.135:3306
Source Database       : lbtcpool

Target Server Type    : MYSQL
Target Server Version : 50721
File Encoding         : 65001

Date: 2018-03-12 10:42:45
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for lbtcnodes
-- ----------------------------
DROP TABLE IF EXISTS `lbtcnodes`;
CREATE TABLE `lbtcnodes` (
  `NodeID` int(11) NOT NULL,
  `NodeAddress` varchar(40) NOT NULL,
  `NodeName` varchar(100) NOT NULL,
  `NodeVotes` bigint(20) NOT NULL,
  `GroupID` int(11) NOT NULL DEFAULT '0',
  `GetRate` decimal(10,4) NOT NULL DEFAULT '0.0000' COMMENT '收益率',
  PRIMARY KEY (`NodeID`)
) ;

-- ----------------------------
-- Table structure for nodeincomes
-- ----------------------------
DROP TABLE IF EXISTS `nodeincomes`;
CREATE TABLE `nodeincomes` (
  `InComeID` int(11) NOT NULL AUTO_INCREMENT,
  `NodeAddress` varchar(40) DEFAULT NULL,
  `CheckTime` datetime DEFAULT NULL,
  `GetCoins` decimal(24,9) DEFAULT NULL COMMENT '周期内收益',
  `NowCoins` decimal(24,9) DEFAULT NULL COMMENT '节点检测时。地址持币数',
  PRIMARY KEY (`InComeID`)
) ;


-- ----------------------------
-- Table structure for nodenewblocks
-- ----------------------------
DROP TABLE IF EXISTS `nodenewblocks`;
CREATE TABLE `nodenewblocks` (
  `NewID` varchar(40) NOT NULL,
  `BlockHash` varchar(100) NOT NULL,
  `NodeAddress` varchar(40) DEFAULT NULL,
  `GetCoins` decimal(24,9) DEFAULT NULL,
  `BlockHeight` int(11) DEFAULT NULL,
  `BlockTime` datetime DEFAULT NULL,
  `CreateWay` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`NewID`)
) ;

-- ----------------------------
-- Table structure for nodevotes
-- ----------------------------
DROP TABLE IF EXISTS `nodevotes`;
CREATE TABLE `nodevotes` (
  `VotesID` varchar(40) NOT NULL,
  `VotesAddress` varchar(40) DEFAULT NULL,
  `NodeAddress` varchar(40) DEFAULT NULL,
  `VoteCoins` bigint(20) DEFAULT NULL,
  `JoinTime` datetime DEFAULT NULL,
  PRIMARY KEY (`VotesID`)
) ;


-- ----------------------------
-- Table structure for poolconfig
-- ----------------------------
DROP TABLE IF EXISTS `poolconfig`;
CREATE TABLE `poolconfig` (
  `ConfigID` int(11) NOT NULL AUTO_INCREMENT,
  `PoolName` varchar(100) DEFAULT NULL,
  `PoolLastTime` datetime DEFAULT NULL COMMENT '矿池上一次统计时间',
  `LoopMins` int(11) DEFAULT NULL COMMENT '每隔多少分钟做一次循环',
  `PublicHour` int(11) DEFAULT NULL,
  `IsPublic` char(1) DEFAULT NULL,
  PRIMARY KEY (`ConfigID`)
) ;

-- ----------------------------
-- Records of poolconfig
-- ----------------------------
INSERT INTO `poolconfig` VALUES ('1', 'LBTC矿池', '2018-02-26 15:53:11', '15', null, null);

-- ----------------------------
-- Table structure for poolgroup
-- ----------------------------
DROP TABLE IF EXISTS `poolgroup`;
CREATE TABLE `poolgroup` (
  `GroupID` int(11) NOT NULL AUTO_INCREMENT,
  `GroupDesc` varchar(200) DEFAULT NULL,
  `ManagerAddress` varchar(40) DEFAULT NULL COMMENT '管理（基金）费地址',
  `GoupRole` int(11) DEFAULT '1' COMMENT '规则，1是阶梯分红，2是节点认领',
  `PublicWay` text COMMENT '手续费,coins|fee-coins|fee是阶梯模式，address|fee|fee|fee|ad1:coin1-ad2:coin2表示管理方节点，大户收益，基金收益，散户收益',
  `IsUsed` char(1) DEFAULT NULL,
  PRIMARY KEY (`GroupID`)
) ;

-- ----------------------------
-- Records of poolgroup
-- ----------------------------
INSERT INTO `poolgroup` VALUES ('1', 'lbtcfans', '17U1L4KqW1fbffG2XJdsCN5nSn2hBsAWNS', '1', '0|0', '1');
INSERT INTO `poolgroup` VALUES ('2', 'lbtcelite.com', '17U1L4KqW1fbffG2XJdsCN5nSn2hBsAWNS', '2', '17U1L4KqW1fbffG2XJdsCN5nSn2hBsAWNS|70|5|25|17U1L4KqW1fbffG2XJdsCN5nSn2hBsAWNS:5-12CacXV2WvdUNBu9K3Z1CUBvfEizPq9r4D:5', '1');

-- ----------------------------
-- Table structure for poolnodes
-- ----------------------------
DROP TABLE IF EXISTS `poolnodes`;
CREATE TABLE `poolnodes` (
  `NodeID` int(11) NOT NULL AUTO_INCREMENT,
  `NodeAddress` varchar(40) DEFAULT NULL,
  `NodeName` varchar(100) DEFAULT NULL,
  `NodeGroupID` int(11) DEFAULT NULL,
  `MakeNewBlockHash` varchar(100) DEFAULT NULL COMMENT '最新出块hash值',
  `MakeNewCoins` decimal(24,9) DEFAULT NULL COMMENT '出块总奖励币数',
  `MakeShareCoins` decimal(24,9) DEFAULT NULL COMMENT '出块总分红币数',
  `CheckTime` datetime DEFAULT NULL COMMENT '检查时间',
  `ThisNewCoins` decimal(24,9) DEFAULT NULL,
  `ThisShareCoins` decimal(24,9) DEFAULT NULL,
  `OwerName` varchar(20) DEFAULT NULL COMMENT '节点所有者',
  `OwerVoteAddress` varchar(40) DEFAULT NULL COMMENT '节点所有者投票地址',
  `VoteMinCoins` int(11) DEFAULT NULL COMMENT '所有者兜底票数',
  `OwerSendAddress` varchar(40) DEFAULT NULL COMMENT '节点所有者收益地址',
  PRIMARY KEY (`NodeID`)
) ;

-- ----------------------------
-- Records of poolnodes
-- ----------------------------
INSERT INTO `poolnodes` VALUES ('1', '1MHawh1LPdi3ZMWns9XsfaBwLhYFeKMnvZ', 'Dragon', '2', 'f28c79daa40f1974bb71d7af0186818048a6928b0df247e71f6803160735390e', '0.000000000', '0.000000000', '2018-03-01 00:59:26', '0.000000000', '0.000000000', 'Vison', '17U1L4KqW1fbffG2XJdsCN5nSn2hBsAWNS', '0', '17U1L4KqW1fbffG2XJdsCN5nSn2hBsAWNS');
INSERT INTO `poolnodes` VALUES ('2', '1BgEszHUTyQuHGxmtwJyrtfe4tk9LVzyQB', 'Zoro', '1', 'f28c79daa40f1974bb71d7af0186818048a6928b0df247e71f6803160735390e', '0.000000000', '0.000000000', '2018-03-01 12:23:34', '0.000000000', '0.000000000', 'Vison', '17U1L4KqW1fbffG2XJdsCN5nSn2hBsAWNS', '0', null);

-- ----------------------------
-- Table structure for useraddresses
-- ----------------------------
DROP TABLE IF EXISTS `useraddresses`;
CREATE TABLE `useraddresses` (
  `UserAddress` varchar(40) NOT NULL,
  `JoinTime` datetime DEFAULT NULL,
  `JoinUserID` int(11) DEFAULT '0',
  PRIMARY KEY (`UserAddress`)
) ;


-- ----------------------------
-- Table structure for userincomeshis
-- ----------------------------
DROP TABLE IF EXISTS `userincomeshis`;
CREATE TABLE `userincomeshis` (
  `InComeID` varchar(40) NOT NULL,
  `NodeAddresses` text NOT NULL,
  `UserAddress` varchar(40) NOT NULL,
  `SetTime` datetime DEFAULT NULL,
  `GetCoins` decimal(24,9) DEFAULT NULL COMMENT '每一次检测的实时收益',
  `TransactionHash` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`InComeID`)
) ;

-- ----------------------------
-- Table structure for userincomesonline
-- ----------------------------
DROP TABLE IF EXISTS `userincomesonline`;
CREATE TABLE `userincomesonline` (
  `InComeID` varchar(40) NOT NULL,
  `NodeAddress` varchar(40) NOT NULL,
  `UserAddress` varchar(40) NOT NULL,
  `CheckTime` datetime DEFAULT NULL,
  `GetCoins` decimal(24,9) DEFAULT NULL COMMENT '每一次检测的实时收益',
  `BlockHeight` int(11) DEFAULT NULL COMMENT '块高',
  PRIMARY KEY (`InComeID`)
) ;

