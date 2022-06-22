--CREATE DATABASE employeedbs
--go

USE employeedbs
go

/***************************************** CREATE TABLE **********************************************************/
if(OBJECT_ID('HOLIDAY') is not null)
	drop table HOLIDAY
go
CREATE TABLE HOLIDAY
(
    holidayID int IDENTITY(1,1) NOT NULL,
	reason nvarchar(255),
	restDays int default 0,
	createdDate datetime default getdate(),
	startedDate datetime,
	endDate datetime,
	empID int,
	primary key (holidayID)
)



if(OBJECT_ID('WORK_TIME') is not null)
	drop table WORK_TIME
go
CREATE TABLE WORK_TIME
(
     workTimeID int IDENTITY(1,1) NOT NULL,
	 startWorkAt datetime,
	 stopWorkAt datetime,
	 workHour int,
	 createdDate datetime default getdate(),
	 empID int, --FK
	 primary key (workTimeID)
)


if(OBJECT_ID('POSITION') is not null)
	drop table POSITION
go
CREATE TABLE POSITION
(
   positionID int IDENTITY(1,1) NOT NULL,
   positionName nvarchar(255),
   primary key (positionID)
)

if(OBJECT_ID('DEPARTMENT') is not null)
	drop table DEPARTMENT
go
CREATE TABLE DEPARTMENT
(
   deptID int IDENTITY(1,1) NOT NULL,
   deptName  nvarchar(255)
   primary key (deptID)
)

if(OBJECT_ID('BENEFIT') is not null)
	drop table BENEFIT
go
CREATE TABLE BENEFIT
(
   benefitID int IDENTITY(1,1) NOT NULL,
   benefitName nvarchar(255),
   enrollNumber int default 0,
   primary key (benefitID)
)


if(OBJECT_ID('EMPLOYEE') is not null)
	drop table EMPLOYEE
go
CREATE TABLE EMPLOYEE
(
   empID int IDENTITY(1,1) NOT NULL,
   fullName nvarchar(255),
   birthDay datetime,
   address nvarchar(255),
   identityCard varchar(45) UNIQUE,
   phone varchar(12) UNIQUE,
   sex varchar(2),
   positionID int,
   deptID int,
   benefitID int,
   empRank nvarchar(125),
   primary key (empID)
)


if(OBJECT_ID('SALARY') is not null)
	drop table SALARY
go
CREATE TABLE SALARY
(
   createdDate date not null,
   empId int not null, --fk
   salary int default 0,
   salaryDeduction int default 0,
   salaryToPay int,
   primary key (createdDate,empId)
)


alter table HOLIDAY 
add constraint FK_HOLIDAY_EMPLOYEE
foreign key (empID) references EMPLOYEE(empID)
ON DELETE CASCADE;

alter table WORK_TIME
add constraint FK_WORK_TIME_EMPLOYEE
foreign key (empID) references EMPLOYEE(empID)
ON DELETE CASCADE;

alter table EMPLOYEE 
add constraint FK_EMPLOYEE_POSITION
foreign key (positionID) references POSITION(positionID)
ON DELETE CASCADE;

alter table EMPLOYEE 
add constraint FK_EMPLOYEE_DEPARTMENT
foreign key (deptID) references DEPARTMENT(deptID)
ON DELETE CASCADE;

alter table EMPLOYEE 
add constraint FK_EMPLOYEE_BENEFIT
foreign key (benefitID) references BENEFIT(benefitID)
ON DELETE CASCADE;

alter table SALARY 
add constraint FK_SALARY_EMPLOYEE
foreign key (empId) references EMPLOYEE(empID)
ON DELETE CASCADE;

/***************************************** INSERT DATA **********************************************************/
-- POSITION
insert into POSITION(positionName)
			VALUES(N'ປະທານ');
insert into POSITION(positionName)
			VALUES(N'ຮອງປະທານ');
insert into POSITION(positionName)
			VALUES(N'ຮອງປະທານກຳມະບານ');
insert into POSITION(positionName)
			VALUES(N'ປະທານຜູ້ບໍລິຫານລະດັບສູງ');

-- DEPARTMENT
insert into DEPARTMENT(deptName)
			VALUES(N'ພະແນກ ຜູ້ຈັດການ');
insert into DEPARTMENT(deptName)
			VALUES(N'ພະແນກ ການຜະລິດ');
insert into DEPARTMENT(deptName)
			VALUES(N'ພະແນກ ການຕະຫຼາດ');
insert into DEPARTMENT(deptName)
			VALUES(N'ພະແນກ ການເງິນ');
insert into DEPARTMENT(deptName)
			VALUES(N'ພະແນກ ບຸກຄົນ');
insert into DEPARTMENT(deptName)
			VALUES(N'ພະແນກ ການຂາຍ');

-- BENEFIT
insert into BENEFIT(benefitName,enrollNumber)
			VALUES(N'ອຸບັດເຫດ',4);
insert into BENEFIT(benefitName,enrollNumber)
			VALUES(N'ຕາຍ',2);

insert into EMPLOYEE(fullName,birthDay,address,identityCard,phone,sex,positionID,deptID, benefitID)
			VALUES(N'ຈັນສະໝອນ','1996-12-12', N'ໜອງທາເໜືອ','1203456781','02098765561','FM',1,1,1)
insert into EMPLOYEE(fullName,birthDay,address,identityCard,phone,sex,positionID,deptID, benefitID)
			VALUES(N'ປັນຍາວົງ','1995-12-12', N'ທາດຫຼວງ','1203456782','02098765562','M',2,1,2)
insert into EMPLOYEE(fullName,birthDay,address,identityCard,phone,sex,positionID,deptID, benefitID)
			VALUES(N'ສິບຸນເຮືອງ','1999-12-12', N'ດອນກອຍ','1203456783','02098765563','FM',1,2,1)
insert into EMPLOYEE(fullName,birthDay,address,identityCard,phone,sex,positionID,deptID, benefitID)
			VALUES(N'ໂອປີ','1994-12-12', N'ທ່າມ່ວງ','1203456784','02098765564','M',3,3,1)
insert into EMPLOYEE(fullName,birthDay,address,identityCard,phone,sex,positionID,deptID, benefitID)
			VALUES(N'ບຸນສີ','1994-12-12', N'ທ່າມ່ວງ','1203456785','02098765565','M',4,4,2)
insert into EMPLOYEE(fullName,birthDay,address,identityCard,phone,sex,positionID,deptID, benefitID)
			VALUES(N'ພອນປະສົງ','1994-12-12', N'ທ່າມ່ວງ','1203456786','02098765566','M',4,5,1)


-- HOLIDAY
insert into HOLIDAY(reason,restDays, startedDate,endDate,empID)
			VALUES(N'ອຸບັດເຫດ',7,'2021-01-01', '2023-01-07',1)
insert into HOLIDAY(reason,restDays, startedDate,endDate, empID)
			VALUES(N'ອຸບັດເຫດ',7,'2021-01-01', '2023-01-07',2)

-- WORK_TIME
--insert into WORK_TIME(reason,restDays, startedDate,endDate,empID)
--			VALUES(N'ອຸບັດເຫດ',7,'2021-01-01', '2023-01-07',1)
--insert into WORK_TIME(reason,restDays, startedDate,endDate, empID)
--			VALUES(N'ອຸບັດເຫດ',7,'2021-01-01', '2023-01-07',2)
