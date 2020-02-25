CREATE TABLE news_launcher (
	id int IDENTITY(1,1) PRIMARY KEY,
	title varchar(255) NOT NULL,
	message varchar(max) NOT NULL,
	newslink varchar(255) NOT NULL,
	imagelink varchar(255) NOT NULL,
	realms varchar(255) NOT NULL,
	date_time varchar(255) NOT NULL
)

CREATE TABLE hot_news (
	id int IDENTITY(1, 1) PRIMARY KEY,
	message text,
	realms varchar(255) NOT NULL
)

CREATE TABLE tab_icon (
	guid int DEFAULT((0)) NOT NULL,
	icon varchar(255) DEFAULT NULL,
)