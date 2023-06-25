# StudyProj
1. Вам нужно создать таблички (скрипт ниже) в СУБД PgAdmin
```
Create table Logs
(
	ID_Logs SERIAL constraint PK_ID_Logs primary key, 
 	IP_Logs varchar(15) not null,
	datetime_Logs varchar not null,
	Request_Logs varchar not null,
	http_status_Logs varchar not null
);

Create table Users
(
	ID_Users SERIAL constraint PK_ID_Users primary key,
	Name_Users varchar not null,
	Surname_Users varchar not null,
	MiddleName_Users varchar not null,
	Password_Users varchar not null
);
```

2. Поменять в файле App.config некоторые данные, а именно путь к файлу с access логами Apache и информаия о бд
