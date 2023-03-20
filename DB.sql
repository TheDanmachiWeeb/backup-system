USE 3b1_podskalskyjakub_db1;

CREATE TABLE Users (
  userId int PRIMARY KEY AUTO_INCREMENT,
  username varchar(255) UNIQUE NOT NULL,
  passwordHash varchar(255) NOT NULL
);

CREATE TABLE Stations (
  stationId int PRIMARY KEY AUTO_INCREMENT,
  stationName varchar(255) NOT NULL,
  ipAddress varchar(45) NOT NULL,
  macAddress varchar(17) NOT NULL,
  active Bit NOT NULL
);

CREATE TABLE Groups (
  groupId int PRIMARY KEY AUTO_INCREMENT,
  groupName varchar(255)
);

CREATE TABLE StationGroup (
  stationId int NOT NULL,
  groupId int NOT NULL,
  PRIMARY KEY(stationId, groupId)
);

CREATE TABLE Configurations (
  configId int PRIMARY KEY AUTO_INCREMENT,
  backupType enum('full', 'diff', 'inc') NOT NULL,
  retention int NOT NULL,
  packageSize int,
  periodCron varchar(255)
);

CREATE TABLE BackupSources (
  configId int NOT NULL,
  sourcePath varchar(255) NOT NULL
);

CREATE TABLE BackupDestinations (
  configId int NOT NULL,
  destinationType enum('local', 'network', 'ftp') NOT NULL,
  destinationPath varchar(255) NOT NULL
);

CREATE TABLE StationConfiguration (
  stationId int NOT NULL,
  groupId int,
  configId int NOT NULL,
  PRIMARY KEY(stationId, configId)
);

CREATE TABLE Reports (
  reportId int PRIMARY KEY AUTO_INCREMENT,
  stationId int NOT NULL,
  configId int NOT NULL,
  reportTime datetime NOT NULL,
  backupSize bigint NOT NULL,
  success Bit NOT NULL
);

ALTER TABLE StationGroup ADD FOREIGN KEY (stationId) REFERENCES Stations (stationId);

ALTER TABLE StationGroup ADD FOREIGN KEY (groupId) REFERENCES Groups (groupId);

ALTER TABLE BackupSources ADD FOREIGN KEY (configId) REFERENCES Configurations (configId);

ALTER TABLE BackupDestinations ADD FOREIGN KEY (configId) REFERENCES Configurations (configId);

ALTER TABLE StationConfiguration ADD FOREIGN KEY (stationId) REFERENCES Stations (stationId);

ALTER TABLE StationConfiguration ADD FOREIGN KEY (groupId) REFERENCES Groups (groupId);

ALTER TABLE StationConfiguration ADD FOREIGN KEY (configId) REFERENCES Configurations (configId);

ALTER TABLE Reports ADD FOREIGN KEY (stationId) REFERENCES Stations (stationId);

ALTER TABLE Reports ADD FOREIGN KEY (configId) REFERENCES Configurations (configId);
