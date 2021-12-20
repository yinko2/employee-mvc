/*
SQLyog Community v13.1.8 (64 bit)
MySQL - 8.0.27 : Database - employee
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`employee` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `employee`;

/*Table structure for table `__efmigrationshistory` */

DROP TABLE IF EXISTS `__efmigrationshistory`;

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `__efmigrationshistory` */

insert  into `__efmigrationshistory`(`MigrationId`,`ProductVersion`) values 
('20211220021644_InitialIdentityServerMigration','6.0.1'),
('20211220021813_InitialIdentityServerMigration','6.0.1'),
('20211220030924_InitialIdentityServerMigration','6.0.1');

/*Table structure for table `apiresourceclaims` */

DROP TABLE IF EXISTS `apiresourceclaims`;

CREATE TABLE `apiresourceclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ApiResourceId` int NOT NULL,
  `Type` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ApiResourceClaims_ApiResourceId` (`ApiResourceId`),
  CONSTRAINT `FK_ApiResourceClaims_ApiResources_ApiResourceId` FOREIGN KEY (`ApiResourceId`) REFERENCES `apiresources` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `apiresourceclaims` */

insert  into `apiresourceclaims`(`Id`,`ApiResourceId`,`Type`) values 
(1,1,'role');

/*Table structure for table `apiresourceproperties` */

DROP TABLE IF EXISTS `apiresourceproperties`;

CREATE TABLE `apiresourceproperties` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ApiResourceId` int NOT NULL,
  `Key` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ApiResourceProperties_ApiResourceId` (`ApiResourceId`),
  CONSTRAINT `FK_ApiResourceProperties_ApiResources_ApiResourceId` FOREIGN KEY (`ApiResourceId`) REFERENCES `apiresources` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `apiresourceproperties` */

/*Table structure for table `apiresources` */

DROP TABLE IF EXISTS `apiresources`;

CREATE TABLE `apiresources` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Enabled` bit(1) NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DisplayName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AllowedAccessTokenSigningAlgorithms` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ShowInDiscoveryDocument` bit(1) NOT NULL,
  `Created` datetime(6) NOT NULL,
  `Updated` datetime(6) DEFAULT NULL,
  `LastAccessed` datetime(6) DEFAULT NULL,
  `NonEditable` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_ApiResources_Name` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `apiresources` */

insert  into `apiresources`(`Id`,`Enabled`,`Name`,`DisplayName`,`Description`,`AllowedAccessTokenSigningAlgorithms`,`ShowInDiscoveryDocument`,`Created`,`Updated`,`LastAccessed`,`NonEditable`) values 
(1,'','EmployeeMVC','EmployeeMVC',NULL,NULL,'','2021-12-20 16:25:33.096884',NULL,NULL,'\0');

/*Table structure for table `apiresourcescopes` */

DROP TABLE IF EXISTS `apiresourcescopes`;

CREATE TABLE `apiresourcescopes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Scope` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ApiResourceId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ApiResourceScopes_ApiResourceId` (`ApiResourceId`),
  CONSTRAINT `FK_ApiResourceScopes_ApiResources_ApiResourceId` FOREIGN KEY (`ApiResourceId`) REFERENCES `apiresources` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `apiresourcescopes` */

insert  into `apiresourcescopes`(`Id`,`Scope`,`ApiResourceId`) values 
(1,'EmployeeMVC.read',1),
(2,'EmployeeMVC.write',1);

/*Table structure for table `apiresourcesecrets` */

DROP TABLE IF EXISTS `apiresourcesecrets`;

CREATE TABLE `apiresourcesecrets` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ApiResourceId` int NOT NULL,
  `Description` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Value` varchar(4000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Expiration` datetime(6) DEFAULT NULL,
  `Type` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Created` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ApiResourceSecrets_ApiResourceId` (`ApiResourceId`),
  CONSTRAINT `FK_ApiResourceSecrets_ApiResources_ApiResourceId` FOREIGN KEY (`ApiResourceId`) REFERENCES `apiresources` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `apiresourcesecrets` */

insert  into `apiresourcesecrets`(`Id`,`ApiResourceId`,`Description`,`Value`,`Expiration`,`Type`,`Created`) values 
(1,1,NULL,'DbsYVAAscQ1HaJn5nUwONjP7UzJJclRpWGn/GKOKSw8=',NULL,'SharedSecret','2021-12-20 16:25:33.097012');

/*Table structure for table `apiscopeclaims` */

DROP TABLE IF EXISTS `apiscopeclaims`;

CREATE TABLE `apiscopeclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ScopeId` int NOT NULL,
  `Type` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ApiScopeClaims_ScopeId` (`ScopeId`),
  CONSTRAINT `FK_ApiScopeClaims_ApiScopes_ScopeId` FOREIGN KEY (`ScopeId`) REFERENCES `apiscopes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `apiscopeclaims` */

/*Table structure for table `apiscopeproperties` */

DROP TABLE IF EXISTS `apiscopeproperties`;

CREATE TABLE `apiscopeproperties` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ScopeId` int NOT NULL,
  `Key` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ApiScopeProperties_ScopeId` (`ScopeId`),
  CONSTRAINT `FK_ApiScopeProperties_ApiScopes_ScopeId` FOREIGN KEY (`ScopeId`) REFERENCES `apiscopes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `apiscopeproperties` */

/*Table structure for table `apiscopes` */

DROP TABLE IF EXISTS `apiscopes`;

CREATE TABLE `apiscopes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Enabled` bit(1) NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DisplayName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Required` bit(1) NOT NULL,
  `Emphasize` bit(1) NOT NULL,
  `ShowInDiscoveryDocument` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_ApiScopes_Name` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `apiscopes` */

insert  into `apiscopes`(`Id`,`Enabled`,`Name`,`DisplayName`,`Description`,`Required`,`Emphasize`,`ShowInDiscoveryDocument`) values 
(1,'','EmployeeMVC.read','EmployeeMVC.read',NULL,'\0','\0',''),
(2,'','EmployeeMVC.write','EmployeeMVC.write',NULL,'\0','\0','');

/*Table structure for table `aspnetroleclaims` */

DROP TABLE IF EXISTS `aspnetroleclaims`;

CREATE TABLE `aspnetroleclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `aspnetroleclaims` */

/*Table structure for table `aspnetroles` */

DROP TABLE IF EXISTS `aspnetroles`;

CREATE TABLE `aspnetroles` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `aspnetroles` */

/*Table structure for table `aspnetuserclaims` */

DROP TABLE IF EXISTS `aspnetuserclaims`;

CREATE TABLE `aspnetuserclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `aspnetuserclaims` */

insert  into `aspnetuserclaims`(`Id`,`UserId`,`ClaimType`,`ClaimValue`) values 
(1,'987ac35b-fd35-48c4-84d1-0352d44d1202','name','Alice Smith'),
(2,'987ac35b-fd35-48c4-84d1-0352d44d1202','given_name','Alice'),
(3,'987ac35b-fd35-48c4-84d1-0352d44d1202','family_name','Smith'),
(4,'987ac35b-fd35-48c4-84d1-0352d44d1202','website','http://alice.com'),
(5,'5898b681-3a4a-4be8-b34b-d30f9107d982','name','Bob Smith'),
(6,'5898b681-3a4a-4be8-b34b-d30f9107d982','given_name','Bob'),
(7,'5898b681-3a4a-4be8-b34b-d30f9107d982','family_name','Smith'),
(8,'5898b681-3a4a-4be8-b34b-d30f9107d982','website','http://bob.com'),
(9,'5898b681-3a4a-4be8-b34b-d30f9107d982','location','somewhere');

/*Table structure for table `aspnetuserlogins` */

DROP TABLE IF EXISTS `aspnetuserlogins`;

CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `aspnetuserlogins` */

/*Table structure for table `aspnetuserroles` */

DROP TABLE IF EXISTS `aspnetuserroles`;

CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `aspnetuserroles` */

/*Table structure for table `aspnetusers` */

DROP TABLE IF EXISTS `aspnetusers`;

CREATE TABLE `aspnetusers` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` bit(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumberConfirmed` bit(1) NOT NULL,
  `TwoFactorEnabled` bit(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` bit(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `aspnetusers` */

insert  into `aspnetusers`(`Id`,`UserName`,`NormalizedUserName`,`Email`,`NormalizedEmail`,`EmailConfirmed`,`PasswordHash`,`SecurityStamp`,`ConcurrencyStamp`,`PhoneNumber`,`PhoneNumberConfirmed`,`TwoFactorEnabled`,`LockoutEnd`,`LockoutEnabled`,`AccessFailedCount`) values 
('5898b681-3a4a-4be8-b34b-d30f9107d982','bob','BOB','BobSmith@email.com','BOBSMITH@EMAIL.COM','','AQAAAAEAACcQAAAAECl9k2O1F65Flh3QgERxOfvkEg5mNCb6Lh3D+F7sipy6abwy6dmdmU9sk4I4sAqIyw==','O3A2TZUUXSUV5DKKMMXGD6NHND5STX5N','25c510da-8506-4174-83fe-b79b8107c2ac',NULL,'\0','\0',NULL,'',0),
('987ac35b-fd35-48c4-84d1-0352d44d1202','alice','ALICE','AliceSmith@email.com','ALICESMITH@EMAIL.COM','','AQAAAAEAACcQAAAAEORM/zUSvU8yuZ2iJWOLLzeAZJYHOleAA6Hyk7daXbDFU3TWNpCPwDdqUdHR9X4+XQ==','67GBL3KTQBAVXWZVJMD4V7YFOY3EUORO','67dd204e-fd5b-49e9-b7a0-622de30f7688',NULL,'\0','\0',NULL,'',0);

/*Table structure for table `aspnetusertokens` */

DROP TABLE IF EXISTS `aspnetusertokens`;

CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `aspnetusertokens` */

/*Table structure for table `clientclaims` */

DROP TABLE IF EXISTS `clientclaims`;

CREATE TABLE `clientclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClientId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ClientClaims_ClientId` (`ClientId`),
  CONSTRAINT `FK_ClientClaims_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `clientclaims` */

/*Table structure for table `clientcorsorigins` */

DROP TABLE IF EXISTS `clientcorsorigins`;

CREATE TABLE `clientcorsorigins` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Origin` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClientId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ClientCorsOrigins_ClientId` (`ClientId`),
  CONSTRAINT `FK_ClientCorsOrigins_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `clientcorsorigins` */

/*Table structure for table `clientgranttypes` */

DROP TABLE IF EXISTS `clientgranttypes`;

CREATE TABLE `clientgranttypes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `GrantType` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClientId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ClientGrantTypes_ClientId` (`ClientId`),
  CONSTRAINT `FK_ClientGrantTypes_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `clientgranttypes` */

insert  into `clientgranttypes`(`Id`,`GrantType`,`ClientId`) values 
(1,'client_credentials',1),
(2,'authorization_code',2);

/*Table structure for table `clientidprestrictions` */

DROP TABLE IF EXISTS `clientidprestrictions`;

CREATE TABLE `clientidprestrictions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Provider` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClientId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ClientIdPRestrictions_ClientId` (`ClientId`),
  CONSTRAINT `FK_ClientIdPRestrictions_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `clientidprestrictions` */

/*Table structure for table `clientpostlogoutredirecturis` */

DROP TABLE IF EXISTS `clientpostlogoutredirecturis`;

CREATE TABLE `clientpostlogoutredirecturis` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `PostLogoutRedirectUri` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClientId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ClientPostLogoutRedirectUris_ClientId` (`ClientId`),
  CONSTRAINT `FK_ClientPostLogoutRedirectUris_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `clientpostlogoutredirecturis` */

insert  into `clientpostlogoutredirecturis`(`Id`,`PostLogoutRedirectUri`,`ClientId`) values 
(1,'https://localhost:5444/signout-callback-oidc',2);

/*Table structure for table `clientproperties` */

DROP TABLE IF EXISTS `clientproperties`;

CREATE TABLE `clientproperties` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ClientId` int NOT NULL,
  `Key` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ClientProperties_ClientId` (`ClientId`),
  CONSTRAINT `FK_ClientProperties_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `clientproperties` */

/*Table structure for table `clientredirecturis` */

DROP TABLE IF EXISTS `clientredirecturis`;

CREATE TABLE `clientredirecturis` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RedirectUri` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClientId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ClientRedirectUris_ClientId` (`ClientId`),
  CONSTRAINT `FK_ClientRedirectUris_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `clientredirecturis` */

insert  into `clientredirecturis`(`Id`,`RedirectUri`,`ClientId`) values 
(1,'https://localhost:5444/signin-oidc',2);

/*Table structure for table `clients` */

DROP TABLE IF EXISTS `clients`;

CREATE TABLE `clients` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Enabled` bit(1) NOT NULL,
  `ClientId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProtocolType` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RequireClientSecret` bit(1) NOT NULL,
  `ClientName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClientUri` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `LogoUri` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `RequireConsent` bit(1) NOT NULL,
  `AllowRememberConsent` bit(1) NOT NULL,
  `AlwaysIncludeUserClaimsInIdToken` bit(1) NOT NULL,
  `RequirePkce` bit(1) NOT NULL,
  `AllowPlainTextPkce` bit(1) NOT NULL,
  `RequireRequestObject` bit(1) NOT NULL,
  `AllowAccessTokensViaBrowser` bit(1) NOT NULL,
  `FrontChannelLogoutUri` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `FrontChannelLogoutSessionRequired` bit(1) NOT NULL,
  `BackChannelLogoutUri` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `BackChannelLogoutSessionRequired` bit(1) NOT NULL,
  `AllowOfflineAccess` bit(1) NOT NULL,
  `IdentityTokenLifetime` int NOT NULL,
  `AllowedIdentityTokenSigningAlgorithms` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AccessTokenLifetime` int NOT NULL,
  `AuthorizationCodeLifetime` int NOT NULL,
  `ConsentLifetime` int DEFAULT NULL,
  `AbsoluteRefreshTokenLifetime` int NOT NULL,
  `SlidingRefreshTokenLifetime` int NOT NULL,
  `RefreshTokenUsage` int NOT NULL,
  `UpdateAccessTokenClaimsOnRefresh` bit(1) NOT NULL,
  `RefreshTokenExpiration` int NOT NULL,
  `AccessTokenType` int NOT NULL,
  `EnableLocalLogin` bit(1) NOT NULL,
  `IncludeJwtId` bit(1) NOT NULL,
  `AlwaysSendClientClaims` bit(1) NOT NULL,
  `ClientClaimsPrefix` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PairWiseSubjectSalt` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Created` datetime(6) NOT NULL,
  `Updated` datetime(6) DEFAULT NULL,
  `LastAccessed` datetime(6) DEFAULT NULL,
  `UserSsoLifetime` int DEFAULT NULL,
  `UserCodeType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `DeviceCodeLifetime` int NOT NULL,
  `NonEditable` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Clients_ClientId` (`ClientId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `clients` */

insert  into `clients`(`Id`,`Enabled`,`ClientId`,`ProtocolType`,`RequireClientSecret`,`ClientName`,`Description`,`ClientUri`,`LogoUri`,`RequireConsent`,`AllowRememberConsent`,`AlwaysIncludeUserClaimsInIdToken`,`RequirePkce`,`AllowPlainTextPkce`,`RequireRequestObject`,`AllowAccessTokensViaBrowser`,`FrontChannelLogoutUri`,`FrontChannelLogoutSessionRequired`,`BackChannelLogoutUri`,`BackChannelLogoutSessionRequired`,`AllowOfflineAccess`,`IdentityTokenLifetime`,`AllowedIdentityTokenSigningAlgorithms`,`AccessTokenLifetime`,`AuthorizationCodeLifetime`,`ConsentLifetime`,`AbsoluteRefreshTokenLifetime`,`SlidingRefreshTokenLifetime`,`RefreshTokenUsage`,`UpdateAccessTokenClaimsOnRefresh`,`RefreshTokenExpiration`,`AccessTokenType`,`EnableLocalLogin`,`IncludeJwtId`,`AlwaysSendClientClaims`,`ClientClaimsPrefix`,`PairWiseSubjectSalt`,`Created`,`Updated`,`LastAccessed`,`UserSsoLifetime`,`UserCodeType`,`DeviceCodeLifetime`,`NonEditable`) values 
(1,'','m2m.client','oidc','','Client Credentials Client',NULL,NULL,NULL,'\0','','\0','','\0','\0','\0',NULL,'',NULL,'','\0',300,NULL,3600,300,NULL,2592000,1296000,1,'\0',1,0,'','','\0','client_',NULL,'2021-12-20 16:25:31.004575',NULL,NULL,NULL,NULL,300,'\0'),
(2,'','interactive','oidc','',NULL,NULL,NULL,NULL,'','','\0','','\0','\0','\0','https://localhost:5444/signout-oidc','',NULL,'','',300,NULL,3600,300,NULL,2592000,1296000,1,'\0',1,0,'','','\0','client_',NULL,'2021-12-20 16:25:31.275616',NULL,NULL,NULL,NULL,300,'\0');

/*Table structure for table `clientscopes` */

DROP TABLE IF EXISTS `clientscopes`;

CREATE TABLE `clientscopes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Scope` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClientId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ClientScopes_ClientId` (`ClientId`),
  CONSTRAINT `FK_ClientScopes_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `clientscopes` */

insert  into `clientscopes`(`Id`,`Scope`,`ClientId`) values 
(1,'EmployeeMVC.read',1),
(2,'EmployeeMVC.write',1),
(3,'openid',2),
(4,'profile',2),
(5,'EmployeeMVC.read',2);

/*Table structure for table `clientsecrets` */

DROP TABLE IF EXISTS `clientsecrets`;

CREATE TABLE `clientsecrets` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ClientId` int NOT NULL,
  `Description` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Value` varchar(4000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Expiration` datetime(6) DEFAULT NULL,
  `Type` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Created` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ClientSecrets_ClientId` (`ClientId`),
  CONSTRAINT `FK_ClientSecrets_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `clientsecrets` */

insert  into `clientsecrets`(`Id`,`ClientId`,`Description`,`Value`,`Expiration`,`Type`,`Created`) values 
(1,1,NULL,'jKGHySpqOJJzXKn9zFr5H09CPujNpVAVgZLP5CGSRq0=',NULL,'SharedSecret','2021-12-20 16:25:31.004945'),
(2,2,NULL,'jKGHySpqOJJzXKn9zFr5H09CPujNpVAVgZLP5CGSRq0=',NULL,'SharedSecret','2021-12-20 16:25:31.275621');

/*Table structure for table `devicecodes` */

DROP TABLE IF EXISTS `devicecodes`;

CREATE TABLE `devicecodes` (
  `UserCode` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DeviceCode` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SubjectId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SessionId` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClientId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Expiration` datetime(6) NOT NULL,
  `Data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`UserCode`),
  UNIQUE KEY `IX_DeviceCodes_DeviceCode` (`DeviceCode`),
  KEY `IX_DeviceCodes_Expiration` (`Expiration`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `devicecodes` */

/*Table structure for table `identityresourceclaims` */

DROP TABLE IF EXISTS `identityresourceclaims`;

CREATE TABLE `identityresourceclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IdentityResourceId` int NOT NULL,
  `Type` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_IdentityResourceClaims_IdentityResourceId` (`IdentityResourceId`),
  CONSTRAINT `FK_IdentityResourceClaims_IdentityResources_IdentityResourceId` FOREIGN KEY (`IdentityResourceId`) REFERENCES `identityresources` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `identityresourceclaims` */

insert  into `identityresourceclaims`(`Id`,`IdentityResourceId`,`Type`) values 
(1,1,'sub'),
(2,2,'name'),
(3,2,'family_name'),
(4,2,'given_name'),
(5,2,'middle_name'),
(6,2,'nickname'),
(7,2,'preferred_username'),
(8,2,'profile'),
(9,2,'picture'),
(10,2,'website'),
(11,2,'gender'),
(12,2,'birthdate'),
(13,2,'zoneinfo'),
(14,2,'locale'),
(15,2,'updated_at'),
(16,3,'role');

/*Table structure for table `identityresourceproperties` */

DROP TABLE IF EXISTS `identityresourceproperties`;

CREATE TABLE `identityresourceproperties` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IdentityResourceId` int NOT NULL,
  `Key` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_IdentityResourceProperties_IdentityResourceId` (`IdentityResourceId`),
  CONSTRAINT `FK_IdentityResourceProperties_IdentityResources_IdentityResourc~` FOREIGN KEY (`IdentityResourceId`) REFERENCES `identityresources` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `identityresourceproperties` */

/*Table structure for table `identityresources` */

DROP TABLE IF EXISTS `identityresources`;

CREATE TABLE `identityresources` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Enabled` bit(1) NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DisplayName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Required` bit(1) NOT NULL,
  `Emphasize` bit(1) NOT NULL,
  `ShowInDiscoveryDocument` bit(1) NOT NULL,
  `Created` datetime(6) NOT NULL,
  `Updated` datetime(6) DEFAULT NULL,
  `NonEditable` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_IdentityResources_Name` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `identityresources` */

insert  into `identityresources`(`Id`,`Enabled`,`Name`,`DisplayName`,`Description`,`Required`,`Emphasize`,`ShowInDiscoveryDocument`,`Created`,`Updated`,`NonEditable`) values 
(1,'','openid','Your user identifier',NULL,'','\0','','2021-12-20 16:25:32.254057',NULL,'\0'),
(2,'','profile','User profile','Your user profile information (first name, last name, etc.)','\0','','','2021-12-20 16:25:32.303652',NULL,'\0'),
(3,'','role',NULL,NULL,'\0','\0','','2021-12-20 16:25:32.312242',NULL,'\0');

/*Table structure for table `persistedgrants` */

DROP TABLE IF EXISTS `persistedgrants`;

CREATE TABLE `persistedgrants` (
  `Key` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SubjectId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SessionId` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClientId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Expiration` datetime(6) DEFAULT NULL,
  `ConsumedTime` datetime(6) DEFAULT NULL,
  `Data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Key`),
  KEY `IX_PersistedGrants_Expiration` (`Expiration`),
  KEY `IX_PersistedGrants_SubjectId_ClientId_Type` (`SubjectId`,`ClientId`,`Type`),
  KEY `IX_PersistedGrants_SubjectId_SessionId_Type` (`SubjectId`,`SessionId`,`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `persistedgrants` */

/*Table structure for table `tbl_employee` */

DROP TABLE IF EXISTS `tbl_employee`;

CREATE TABLE `tbl_employee` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `DateOfBirth` date NOT NULL,
  `Address` varchar(200) DEFAULT NULL,
  `Phone` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Email` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatedTime` datetime DEFAULT NULL,
  `ModifiedTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;

/*Data for the table `tbl_employee` */

insert  into `tbl_employee`(`ID`,`Name`,`DateOfBirth`,`Address`,`Phone`,`Email`,`CreatedTime`,`ModifiedTime`) values 
(1,'Aung Myat Kyaw','1997-04-13','YGN','09451453074','aungmyatkyaw.kk@gmail.com','2021-12-09 13:51:04','2021-12-18 13:51:14'),
(4,'Yin Ko','1997-01-29','Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing','09451453074','aab@b','2021-12-15 14:06:00','2021-12-15 14:06:00'),
(10,'Matrix','2021-12-03','YGN','09451453074','abcd@33.com','2021-12-19 15:55:51','2021-12-20 23:00:40');

/*Table structure for table `tbl_eventlog` */

DROP TABLE IF EXISTS `tbl_eventlog`;

CREATE TABLE `tbl_eventlog` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `LogType` int DEFAULT NULL COMMENT 'Info = 1, Error = 2,Warning = 3, Insert = 4,Update = 5, Delete = 6',
  `LogDateTime` datetime DEFAULT NULL,
  `Source` varchar(100) DEFAULT NULL,
  `FormName` varchar(100) DEFAULT NULL,
  `LogMessage` text,
  `ErrorMessage` text,
  `UserID` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `user_id` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=157 DEFAULT CHARSET=utf8mb3;

/*Data for the table `tbl_eventlog` */

insert  into `tbl_eventlog`(`ID`,`LogType`,`LogDateTime`,`Source`,`FormName`,`LogMessage`,`ErrorMessage`,`UserID`) values 
(65,4,'2021-12-18 08:00:33','/api/Employees','EmployeesController.PostEmployee','Created :\r\nId : 3\r\nName : Yin Ko\r\nDateOfBirth : 12/13/1997 12:00:00 AM\r\nAddress : HTD\r\nPhone : 09951843803\r\nEmail : yinko.152.yk@gmail.com\r\nCreatedTime : 12/9/2021 1:51:04 PM\r\nModifiedTime : 12/18/2021 1:51:14 PM\r\n','',NULL),
(66,4,'2021-12-18 09:52:56','/api/Holidays','HolidaysController.PostHoliday','Created :\r\nDate : 8/24/2021 12:00:00 AM\r\nName : Testing 2\r\n','',NULL),
(67,4,'2021-12-18 09:53:46','/api/Holidays','HolidaysController.PostHoliday','Created :\r\nDate : 9/8/2021 12:00:00 AM\r\nName : Testing 2\r\n','',NULL),
(68,4,'2021-12-18 09:54:55','/api/Holidays','HolidaysController.PostHoliday','Created :\r\nDate : 8/8/2021 12:00:00 AM\r\nName : Testing 2\r\n','',NULL),
(69,5,'2021-12-18 10:01:58','/api/Holidays/2021-12-17','HolidaysController.PutHoliday','Updated :\r\nDate : 12/17/2021 12:00:00 AM\r\nName : Testing 2\r\n','',NULL),
(70,6,'2021-12-18 10:02:45','/api/Holidays/2021-12-17','HolidaysController.DeleteHoliday','Deleted :\r\nDate : 12/17/2021 12:00:00 AM\r\nName : Testing 2\r\n','',NULL),
(71,5,'2021-12-18 10:04:06','/api/Employees/3','EmployeesController.PutEmployee','Updated :\r\nId : 3\r\nName : Yin Ko\r\nDateOfBirth : 12/13/1997 12:00:00 AM\r\nAddress : NPT\r\nPhone : 09951843803\r\nEmail : yinko.152.yk@gmail.com\r\nCreatedTime : 12/9/2021 1:51:04 PM\r\nModifiedTime : 12/18/2021 1:51:14 PM\r\n','',NULL),
(72,6,'2021-12-18 10:05:23','/api/Employees/3','EmployeesController.DeleteEmployee','Deleted :\r\nId : 3\r\nName : Yin Ko\r\nDateOfBirth : 12/13/1997 12:00:00 AM\r\nAddress : NPT\r\nPhone : 09951843803\r\nEmail : yinko.152.yk@gmail.com\r\nCreatedTime : 12/9/2021 1:51:04 PM\r\nModifiedTime : 12/18/2021 1:51:14 PM\r\n','',NULL),
(73,4,'2021-12-18 10:38:31','/api/users/Registration','UsersController.RegisterCashier','Created :\r\nId : 2\r\nUserLevelId : 1\r\nUserName : yinko\r\nPassword : YluqPLNl4qlDHPZMq4ZKSbus548gUwkF\r\nPasswordSalt : ygUz1mkjc6y55UjpNY3++lBGYWPf9olx\r\nCreatedTime : 12/18/2021 10:38:30 AM\r\nModifiedTime : \r\n','',NULL),
(74,3,'2021-12-18 10:41:57','/api/users/Registration','UsersController.RegisterCashier','User with UserName: yinko already exists','',NULL),
(75,3,'2021-12-18 10:42:32','/api/users/Registration','UsersController.RegisterCashier','User with UserName: yinko  already exists','',NULL),
(76,3,'2021-12-18 10:42:56','/api/users/Registration','UsersController.RegisterCashier','User with UserName: admin already exists','',NULL),
(77,4,'2021-12-18 10:43:03','/api/users/Registration','UsersController.RegisterCashier','Created :\r\nId : 3\r\nUserLevelId : 1\r\nUserName : amk\r\nPassword : UMxVM6GuPnXIwicvXojmR5ACgqqOQubV\r\nPasswordSalt : 5u4RUroib5HQJSRZkE19dysgdH4y5Nrk\r\nCreatedTime : 12/18/2021 10:43:03 AM\r\nModifiedTime : \r\n','',NULL),
(78,3,'2021-12-18 10:43:40','/api/users/Registration','UsersController.RegisterCashier','User with UserName: amk already exists','',NULL),
(79,4,'2021-12-18 11:05:13','/api/leaves','LeavesController.PostLeave','Created :\r\nId : 2\r\nEmployeeId : 1\r\nDate : 11/16/2021 12:00:00 AM\r\nReason : Personal\r\nCreatedTime : 11/17/2021 5:32:17 PM\r\nModifiedTime : 11/18/2021 5:32:22 PM\r\n','',NULL),
(80,5,'2021-12-18 11:06:20','/api/leaves/2','LeavesController.PutLeave','Updated :\r\nId : 2\r\nEmployeeId : 1\r\nDate : 11/17/2021 12:00:00 AM\r\nReason : Personal\r\nCreatedTime : 11/17/2021 5:32:17 PM\r\nModifiedTime : 11/18/2021 5:32:22 PM\r\n','',NULL),
(81,6,'2021-12-18 11:06:39','/api/leaves/2','LeavesController.DeleteLeave','Deleted :\r\nId : 2\r\nEmployeeId : 1\r\nDate : 11/17/2021 12:00:00 AM\r\nReason : Personal\r\nCreatedTime : 11/17/2021 5:32:17 PM\r\nModifiedTime : 11/18/2021 5:32:22 PM\r\n','',NULL),
(82,4,'2021-12-18 11:12:27','/api/userlevels','UserLevelsController.PostUserLevel','Created :\r\nId : 0\r\nUserLevel : Test\r\nDescription : \r\n','',NULL),
(83,4,'2021-12-18 11:14:39','/api/userlevels','UserLevelsController.PostUserLevel','Created :\r\nId : 0\r\nUserLevel : Administrator\r\nDescription : can access all features\r\n','',NULL),
(84,4,'2021-12-18 11:14:47','/api/userlevels','UserLevelsController.PostUserLevel','Created :\r\nId : 0\r\nUserLevel : Administrator\r\nDescription : can access all features\r\n','',NULL),
(85,4,'2021-12-18 11:15:44','/api/userlevels','UserLevelsController.PostUserLevel','Created :\r\nId : 0\r\nUserLevel : Administrator\r\nDescription : can access all features\r\n','',NULL),
(86,4,'2021-12-18 11:16:52','/api/userlevels','UserLevelsController.PostUserLevel','Created :\r\nId : 4\r\nUserLevel : Administrator\r\nDescription : can access all features\r\n','',NULL),
(87,4,'2021-12-18 11:17:38','/api/userlevels','UserLevelsController.PostUserLevel','Created :\r\nId : 2\r\nUserLevel : Test\r\nDescription : \r\n','',NULL),
(88,5,'2021-12-18 11:18:09','/api/userlevels/2','UserLevelsController.PutUserLevel','Updated :\r\nId : 2\r\nUserLevel : Test\r\nDescription : test\r\n','',NULL),
(89,6,'2021-12-18 11:18:22','/api/userlevels/2','UserLevelsController.DeleteUserLevel','Deleted :\r\nId : 2\r\nUserLevel : Test\r\nDescription : test\r\n','',NULL),
(90,3,'2021-12-18 11:55:34','/api/Holidays/12-17-2021','','Token not found','',NULL),
(91,3,'2021-12-18 11:55:46','/api/users/2','','Token not found','',NULL),
(92,3,'2021-12-18 11:56:34','/api/Holidays/12-17-2021','','Token not found','',NULL),
(93,2,'2021-12-18 11:56:43','/api/token','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(94,2,'2021-12-18 11:56:57','/api/token','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(95,2,'2021-12-18 11:57:06','/api/token','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(96,2,'2021-12-18 11:57:10','/api/token','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(97,2,'2021-12-18 11:57:20','/api/token','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(98,2,'2021-12-18 11:57:29','/api/token','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(99,2,'2021-12-18 11:57:45','/api/token','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(100,2,'2021-12-18 11:57:50','/api/token','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(101,4,'2021-12-18 11:58:31','/api/users/Registration','UsersController.RegisterCashier','Created :\r\nId : 1\r\nUserLevelId : 1\r\nUserName : admin\r\nPassword : M9qkgXCgPknvqMpXrs9HtbWhWOKut8On\r\nPasswordSalt : jcLL/oVt9Gd/vu6gt8uJykl+EWVw3a0K\r\nCreatedTime : 12/18/2021 11:58:31 AM\r\nModifiedTime : \r\n','',NULL),
(102,2,'2021-12-18 11:58:41','/api/token','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(103,2,'2021-12-18 11:58:50','/api/token','','Failed to read login credentials','Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.',NULL),
(104,1,'2021-12-18 12:04:49','/api/token','','Successful login for this account UserName: admin','',1),
(105,2,'2021-12-18 12:06:33','/api/Holidays/12-17-2021','','Invalid Token, Access Denied.','IDX10503: Signature validation failed. Token does not have a kid. Keys tried: \'System.Text.StringBuilder\'.\nExceptions caught:\n \'System.Text.StringBuilder\'.\ntoken: \'System.IdentityModel.Tokens.Jwt.JwtSecurityToken\'.',NULL),
(106,3,'2021-12-18 12:07:01','/api/Employees','','Token not found','',NULL),
(107,3,'2021-12-18 12:07:28','/api/userlevels','','Token not found','',NULL),
(108,2,'2021-12-18 12:07:40','/api/userlevels','','Invalid Token, Access Denied.','IDX12729: Unable to decode the header \'System.String\' as Base64Url encoded string. jwtEncodedString: \'System.String\'.',NULL),
(109,3,'2021-12-18 12:16:19','/WeatherForecast','','Token not found','',NULL),
(110,3,'2021-12-18 12:21:03','/api/Leaves','','Token not found','',NULL),
(111,1,'2021-12-18 12:21:32','/api/token','','Successful login for this account UserName: admin','',1),
(112,3,'2021-12-18 12:23:43','/api/users/2','','Token not found','',NULL),
(113,2,'2021-12-18 12:23:51','/api/users/2','','Invalid Token, Access Denied.','IDX12729: Unable to decode the header \'System.String\' as Base64Url encoded string. jwtEncodedString: \'System.String\'.',NULL),
(114,1,'2021-12-18 12:24:30','/api/token','','Successful login for this account UserName: admin','',1),
(115,3,'2021-12-19 07:02:03','/api/users/Registration','EmployeeMVC.API.Controllers.UsersController.RegisterCashier','User with UserName: admin already exists','',NULL),
(116,4,'2021-12-19 07:02:18','/api/users/Registration','EmployeeMVC.API.Controllers.UsersController.RegisterCashier','Created :\r\nId : 2\r\nUserLevelId : 1\r\nUserName : yinko\r\nPassword : hbuW7kuMW7M8m9B30LuN11hqWMyulN/h\r\nPasswordSalt : YqO2F8lQA4cylR99viutv3zXSKGcU+JE\r\nCreatedTime : 12/19/2021 7:02:17 AM\r\nModifiedTime : \r\n','',NULL),
(117,4,'2021-12-19 08:36:09','/Employees/Create','Views.EmployeesController.Create','Created :\r\nId : 5\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/3/2021 3:05:00 PM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : aungmyatkyaw.kk@gmail.com\r\nCreatedTime : 12/19/2021 8:36:08 AM\r\nModifiedTime : \r\n','',NULL),
(118,6,'2021-12-19 08:36:54','/Employees/Delete/5','Views.EmployeesController.DeleteConfirmed','Deleted :\r\nId : 5\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : aungmyatkyaw.kk@gmail.com\r\nCreatedTime : 12/19/2021 8:36:09 AM\r\nModifiedTime : \r\n','',NULL),
(119,4,'2021-12-19 09:12:54','/Employees/Create','Views.EmployeesController.Create','Created :\r\nId : 6\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/13/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33\r\nCreatedTime : 12/19/2021 9:12:51 AM\r\nModifiedTime : \r\n','',NULL),
(120,5,'2021-12-19 09:13:18','/Employees/Edit/6','Views.EmployeesController.Edit','Updated :\r\nId : 6\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/13/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33.com\r\nCreatedTime : \r\nModifiedTime : 12/19/2021 9:13:17 AM\r\n','',NULL),
(121,6,'2021-12-19 09:15:41','/Employees/Delete/6','Views.EmployeesController.DeleteConfirmed','Deleted :\r\nId : 6\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/13/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33.com\r\nCreatedTime : \r\nModifiedTime : 12/19/2021 9:13:18 AM\r\n','',NULL),
(122,4,'2021-12-19 09:16:00','/Employees/Create','Views.EmployeesController.Create','Created :\r\nId : 7\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33.com\r\nCreatedTime : 12/19/2021 9:15:59 AM\r\nModifiedTime : \r\n','',NULL),
(123,5,'2021-12-19 09:16:11','/Employees/Edit/7','Views.EmployeesController.Edit','Updated :\r\nId : 7\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33\r\nCreatedTime : \r\nModifiedTime : 12/19/2021 9:16:10 AM\r\n','',NULL),
(124,6,'2021-12-19 09:18:19','/Employees/Delete/7','Views.EmployeesController.DeleteConfirmed','Deleted :\r\nId : 7\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33\r\nCreatedTime : \r\nModifiedTime : 12/19/2021 9:16:11 AM\r\n','',NULL),
(125,4,'2021-12-19 09:18:34','/Employees/Create','Views.EmployeesController.Create','Created :\r\nId : 8\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/2/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33\r\nCreatedTime : 12/19/2021 9:18:33 AM\r\nModifiedTime : \r\n','',NULL),
(126,5,'2021-12-19 09:18:43','/Employees/Edit/8','Views.EmployeesController.Edit','Updated :\r\nId : 8\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/2/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33.com\r\nCreatedTime : \r\nModifiedTime : 12/19/2021 9:18:43 AM\r\n','',NULL),
(127,6,'2021-12-19 09:24:09','/Employees/Delete/8','Views.EmployeesController.DeleteConfirmed','Deleted :\r\nId : 8\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/2/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33.com\r\nCreatedTime : \r\nModifiedTime : 12/19/2021 9:18:43 AM\r\n','',NULL),
(128,4,'2021-12-19 09:24:18','/Employees/Create','Views.EmployeesController.Create','Created :\r\nId : 9\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33\r\nCreatedTime : 12/19/2021 9:24:17 AM\r\nModifiedTime : \r\n','',NULL),
(129,5,'2021-12-19 09:24:22','/Employees/Edit/9','Views.EmployeesController.Edit','Updated :\r\nId : 9\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33.com\r\nCreatedTime : 12/19/2021 9:24:18 AM\r\nModifiedTime : 12/19/2021 9:24:21 AM\r\n','',NULL),
(130,5,'2021-12-19 09:24:35','/Employees/Edit/9','Views.EmployeesController.Edit','Updated :\r\nId : 9\r\nName : Aung Myat Kyaw 2\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33.com\r\nCreatedTime : 12/19/2021 9:24:18 AM\r\nModifiedTime : 12/19/2021 9:24:34 AM\r\n','',NULL),
(131,6,'2021-12-19 09:25:42','/Employees/Delete/9','Views.EmployeesController.DeleteConfirmed','Deleted :\r\nId : 9\r\nName : Aung Myat Kyaw 2\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33.com\r\nCreatedTime : 12/19/2021 9:24:18 AM\r\nModifiedTime : 12/19/2021 9:24:34 AM\r\n','',NULL),
(132,4,'2021-12-19 09:25:51','/Employees/Create','Views.EmployeesController.Create','Created :\r\nId : 10\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33\r\nCreatedTime : 12/19/2021 3:55:51 PM\r\nModifiedTime : \r\n','',NULL),
(133,5,'2021-12-19 09:26:34','/Employees/Edit/10','Views.EmployeesController.Edit','Updated :\r\nId : 10\r\nName : Aung Myat Kyaw\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : Lut Latt Yay Street, Corner of Thukha Street and U Cho Street, Hlaing\r\nPhone : 09451453074\r\nEmail : abcd@33.com\r\nCreatedTime : 12/19/2021 3:55:51 PM\r\nModifiedTime : 12/19/2021 3:56:34 PM\r\n','',NULL),
(134,4,'2021-12-19 09:53:40','/Holidays/Create','Views.HolidaysController.Create','Created :\r\nDate : 12/14/2021 12:00:00 AM\r\nName : abc\r\n','',NULL),
(135,4,'2021-12-19 10:10:12','/Holidays/Create','Views.HolidaysController.Create','Created :\r\nId : 1\r\nDate : 12/3/2021 12:00:00 AM\r\nName : abc\r\n','',NULL),
(136,6,'2021-12-19 10:15:10','/Holidays/Delete/1','Views.HolidaysController.DeleteConfirmed','Deleted :\r\nId : 1\r\nDate : 12/3/2021 12:00:00 AM\r\nName : abc\r\n','',NULL),
(137,4,'2021-12-19 10:15:17','/Holidays/Create','Views.HolidaysController.Create','Created :\r\nId : 2\r\nDate : 12/1/2021 12:00:00 AM\r\nName : Aung Myat Kyaw\r\n','',NULL),
(138,5,'2021-12-19 10:16:24','/Holidays/Edit/2','Views.HolidaysController.Edit','Updated :\r\nId : 2\r\nDate : 12/1/2021 12:00:00 AM\r\nName : Aung Myat Kyaw 2\r\n','',NULL),
(139,4,'2021-12-19 10:17:03','/Holidays/Create','Views.HolidaysController.Create','Created :\r\nId : 3\r\nDate : 12/25/2021 12:00:00 AM\r\nName : abc\r\n','',NULL),
(140,5,'2021-12-19 10:31:59','/Holidays/Edit/2','Views.HolidaysController.Edit','Updated :\r\nId : 2\r\nDate : 12/25/2021 12:00:00 AM\r\nName : Aung Myat Kyaw 2\r\n','',NULL),
(141,5,'2021-12-19 10:42:21','/Holidays/Edit/2','Views.HolidaysController.Edit','Updated :\r\nId : 2\r\nDate : 12/2/2021 12:00:00 AM\r\nName : Aung Myat Kyaw 2\r\n','',NULL),
(142,5,'2021-12-19 10:42:28','/Holidays/Edit/2','Views.HolidaysController.Edit','Updated :\r\nId : 2\r\nDate : 12/25/2021 12:00:00 AM\r\nName : Aung Myat Kyaw 2\r\n','',NULL),
(143,5,'2021-12-19 10:45:00','/Holidays/Edit/2','Views.HolidaysController.Edit','Updated :\r\nId : 2\r\nDate : 12/3/2021 12:00:00 AM\r\nName : Aung Myat Kyaw 2\r\n','',NULL),
(144,4,'2021-12-19 11:35:21','/Leaves/Create','Views.LeavesController.Create','Created :\r\nId : 4\r\nEmployeeId : 1\r\nDate : 12/20/2021 12:00:00 AM\r\nReason : Personal\r\nCreatedTime : 12/19/2021 6:05:19 PM\r\nModifiedTime : \r\n','',NULL),
(145,5,'2021-12-19 11:35:46','/Leaves/Edit/4','Views.LeavesController.Edit','Updated :\r\nId : 4\r\nEmployeeId : 1\r\nDate : 12/20/2021 12:00:00 AM\r\nReason : Personal Business\r\nCreatedTime : 12/19/2021 6:05:20 PM\r\nModifiedTime : 12/19/2021 6:05:45 PM\r\n','',NULL),
(146,5,'2021-12-19 11:37:07','/Leaves/Edit/1','Views.LeavesController.Edit','Updated :\r\nId : 1\r\nEmployeeId : 1\r\nDate : 12/16/2021 12:00:00 AM\r\nReason : illness\r\nCreatedTime : 12/17/2021 5:32:17 PM\r\nModifiedTime : 12/19/2021 6:07:06 PM\r\n','',NULL),
(147,6,'2021-12-19 11:37:22','/Leaves/Delete/4','Views.LeavesController.DeleteConfirmed','Deleted :\r\nId : 4\r\nEmployeeId : 1\r\nDate : 12/20/2021 12:00:00 AM\r\nReason : Personal Business\r\nCreatedTime : 12/19/2021 6:05:20 PM\r\nModifiedTime : 12/19/2021 6:05:45 PM\r\n','',NULL),
(149,5,'2021-12-19 12:23:40','/Leaves/Edit/1','Views.LeavesController.Edit','Updated :\r\nId : 1\r\nEmployeeId : 1\r\nDate : 12/16/2021 12:00:00 AM\r\nReason : illness\r\nCreatedTime : 12/17/2021 5:32:17 PM\r\nModifiedTime : 12/19/2021 6:53:39 PM\r\n','',NULL),
(150,3,'2021-12-19 13:18:35','/Employees','','Token not found','',NULL),
(151,3,'2021-12-19 13:18:40','/Holidays','','Token not found','',NULL),
(152,3,'2021-12-19 13:18:50','/Home/Privacy','','Token not found','',NULL),
(153,3,'2021-12-19 16:05:26','/Employees','','Token not found','',NULL),
(154,4,'2021-12-20 15:57:59','/Holidays/Create','Views.HolidaysController.Create','Created :\r\nId : 4\r\nDate : 11/29/2021 12:00:00 AM\r\nName : Eating\r\n','',NULL),
(155,5,'2021-12-20 16:30:40','/Employees/Edit/10','Views.EmployeesController.Edit','Updated :\r\nId : 10\r\nName : Matrix\r\nDateOfBirth : 12/3/2021 12:00:00 AM\r\nAddress : YGN\r\nPhone : 09451453074\r\nEmail : abcd@33.com\r\nCreatedTime : 12/19/2021 3:55:51 PM\r\nModifiedTime : 12/20/2021 11:00:39 PM\r\n','',NULL),
(156,4,'2021-12-20 16:31:26','/Leaves/Create','Views.LeavesController.Create','Created :\r\nId : 5\r\nEmployeeId : 4\r\nDate : 12/21/2021 12:00:00 AM\r\nReason : Rest\r\nCreatedTime : 12/20/2021 11:01:26 PM\r\nModifiedTime : \r\n','',NULL);

/*Table structure for table `tbl_holiday` */

DROP TABLE IF EXISTS `tbl_holiday`;

CREATE TABLE `tbl_holiday` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `Name` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;

/*Data for the table `tbl_holiday` */

insert  into `tbl_holiday`(`ID`,`Date`,`Name`) values 
(2,'2021-12-03','Dipawali'),
(3,'2021-12-25','Christmas'),
(4,'2021-11-29','Eating');

/*Table structure for table `tbl_leave` */

DROP TABLE IF EXISTS `tbl_leave`;

CREATE TABLE `tbl_leave` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `EmployeeID` int NOT NULL,
  `Date` date NOT NULL,
  `Reason` varchar(100) NOT NULL,
  `CreatedTime` datetime DEFAULT NULL,
  `ModifiedTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `EmployeeID` (`EmployeeID`),
  CONSTRAINT `tbl_leave_ibfk_1` FOREIGN KEY (`EmployeeID`) REFERENCES `tbl_employee` (`ID`) ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;

/*Data for the table `tbl_leave` */

insert  into `tbl_leave`(`ID`,`EmployeeID`,`Date`,`Reason`,`CreatedTime`,`ModifiedTime`) values 
(1,1,'2021-12-16','illness','2021-12-17 17:32:17','2021-12-19 18:53:39'),
(5,4,'2021-12-21','Rest','2021-12-20 23:01:26',NULL);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
