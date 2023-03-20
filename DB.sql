USE 3b1_podskalskyjakub_db1;

CREATE TABLE Users (
  user_id int PRIMARY KEY AUTO_INCREMENT,
  username varchar(255) UNIQUE NOT NULL,
  password_hash varchar(255) NOT NULL
);

CREATE TABLE Stations (
  station_id int PRIMARY KEY AUTO_INCREMENT,
  station_name varchar(255) NOT NULL,
  ip_address varchar(45) NOT NULL,
  mac_address varchar(17) NOT NULL,
  active Bit NOT NULL
);

CREATE TABLE Groups (
  group_id int PRIMARY KEY AUTO_INCREMENT,
  group_name varchar(255)
);

CREATE TABLE StationGroup (
  station_id int NOT NULL,
  group_id int NOT NULL,
  PRIMARY KEY(station_id, group_id)
);

CREATE TABLE Configurations (
  config_id int PRIMARY KEY AUTO_INCREMENT,
  backup_type enum('full', 'diff', 'inc') NOT NULL,
  retention int NOT NULL,
  package_size int,
  period_cron varchar(255)
);

CREATE TABLE BackupSources (
  config_id int NOT NULL,
  source_path varchar(255) NOT NULL
);

CREATE TABLE BackupDestinations (
  config_id int NOT NULL,
  destination_type enum('local', 'network', 'ftp') NOT NULL,
  destination_path varchar(255) NOT NULL
);

CREATE TABLE StationConfiguration (
  station_id int NOT NULL,
  group_id int,
  config_id int NOT NULL,
  PRIMARY KEY(station_id, config_id)
);

CREATE TABLE Reports (
  report_id int PRIMARY KEY AUTO_INCREMENT,
  station_id int NOT NULL,
  config_id int NOT NULL,
  report_time datetime NOT NULL,
  backup_size bigint NOT NULL,
  success Bit NOT NULL
);

ALTER TABLE StationGroup ADD FOREIGN KEY (station_id) REFERENCES Stations (station_id);

ALTER TABLE StationGroup ADD FOREIGN KEY (group_id) REFERENCES Groups (group_id);

ALTER TABLE BackupSources ADD FOREIGN KEY (config_id) REFERENCES Configurations (config_id);

ALTER TABLE BackupDestinations ADD FOREIGN KEY (config_id) REFERENCES Configurations (config_id);

ALTER TABLE StationConfiguration ADD FOREIGN KEY (station_id) REFERENCES Stations (station_id);

ALTER TABLE StationConfiguration ADD FOREIGN KEY (group_id) REFERENCES Groups (group_id);

ALTER TABLE StationConfiguration ADD FOREIGN KEY (config_id) REFERENCES Configurations (config_id);

ALTER TABLE Reports ADD FOREIGN KEY (station_id) REFERENCES Stations (station_id);

ALTER TABLE Reports ADD FOREIGN KEY (config_id) REFERENCES Configurations (config_id);
