-- EMPLOYEE
--! add employee
use employeedbs
go

--! Create employee
if(OBJECT_ID('sp_AddEmployee') is not null)
	drop proc sp_AddEmployee
go
create procedure sp_AddEmployee 
	@fullname nvarchar(255), @birthday datetime, 
	@address nvarchar(255), @identityCard varchar(45), 
	@phone varchar(12), @sex varchar(2),
	@positionID int, @deptID int, @benefitID int
as
begin

	INSERT INTO EMPLOYEE(fullName, birthDay, address, identityCard, phone, sex, positionID, deptID,benefitID)
	VALUES(@fullname, @birthday, @address, @identityCard, @phone, @sex,@positionID, @deptID,@benefitID)

	Update BENEFIT
	set enrollNumber = enrollNumber + 1
	where benefitID = @benefitID


end
GO


--! Delete employee
if(OBJECT_ID('sp_DeleteEmployee') is not null)
	drop proc sp_DeleteEmployee
go
create procedure sp_DeleteEmployee 
	@empId int
as
begin
	delete from EMPLOYEE 
	WHERE empID = @empId
END
GO

--! update employee
if(OBJECT_ID('sp_UpdateEmployee') is not null)
	drop proc sp_UpdateEmployee
go
create procedure sp_UpdateEmployee 
	@empID int, @fullname nvarchar(255), @birthday datetime, 
	@address nvarchar(255), @identityCard varchar(45), 
	@phone varchar(12), @sex varchar(2),
	@positionID int, @deptID int, @benefitID int
as
begin
	UPDATE EMPLOYEE SET fullName = @fullname,
						birthDay = @birthday,
						address = @address,
						identityCard = @identityCard,
						phone = @phone,
						sex = @sex,
						positionID = @positionID,
						deptID = @deptID,
						benefitID = @benefitID
	WHERE empID = @empID
end
go

-- ======================= DEPT ======================= 
if(OBJECT_ID('sp_AddDepartment') is not null)
	drop proc sp_AddDepartment
go
create procedure sp_AddDepartment 
	@deptName nvarchar(255)
as
begin
	INSERT INTO DEPARTMENT(deptName)
	VALUES(@deptName)
end
GO


--! Delete employee
if(OBJECT_ID('sp_DeleteDepartment') is not null)
	drop proc sp_DeleteDepartment
go
create procedure sp_DeleteDepartment 
	@deptID int
as
begin
	delete from DEPARTMENT 
	WHERE deptID = @deptID
END
GO

--! update employee
if(OBJECT_ID('sp_UpdateDepartment') is not null)
	drop proc sp_UpdateDepartment
go
create procedure sp_UpdateDepartment 
	@deptID int, @deptName nvarchar(255)
as
begin
	UPDATE DEPARTMENT SET deptName = @deptName
	WHERE deptID = @deptID
end
go



-- ======================= BENEFIT ======================= 
if(OBJECT_ID('sp_AddBenefit') is not null)
	drop proc sp_AddBenefit
go
create procedure sp_AddBenefit 
	@benefitName nvarchar(255)
as
begin
	INSERT INTO BENEFIT(benefitName)
	VALUES(@benefitName)
end
GO


--! Delete employee
if(OBJECT_ID('sp_DeleteBenefit') is not null)
	drop proc sp_DeleteBenefit
go
create procedure sp_DeleteBenefit
	@benefitID int
as
begin
	delete from BENEFIT 
	WHERE benefitID = @benefitID
END
GO

--! update employee
if(OBJECT_ID('sp_UpdateBenefit') is not null)
	drop proc sp_UpdateBenefit
go
create procedure sp_UpdateBenefit
	@benefitID int, @benefitName nvarchar(255)
as
begin
	UPDATE BENEFIT SET benefitName = @benefitName
	WHERE benefitID = @benefitID
end
go




-- ======================= POSITION ======================= 
if(OBJECT_ID('sp_AddPosition') is not null)
	drop proc sp_AddPosition
go
create procedure sp_AddPosition 
	@positionName nvarchar(255)
as
begin
	INSERT INTO POSITION(positionName)
	VALUES(@positionName)
end
GO


--! Delete employee
if(OBJECT_ID('sp_DeletePosition') is not null)
	drop proc sp_DeletePosition
go
create procedure sp_DeletePosition
	@positionID int
as
begin
	delete from POSITION 
	WHERE positionID = @positionID
END
GO

--! update employee
if(OBJECT_ID('sp_UpdatePosition') is not null)
	drop proc sp_UpdatePosition
go

create procedure sp_UpdatePosition
	@positionID int, @positionName nvarchar(255)
as
begin
	UPDATE POSITION SET positionName = @positionName
	WHERE positionID = @positionID
end
go



-- ======================= HOLIDAY ======================= 
--ADD
if(OBJECT_ID('sp_AddHoliday') is not null)
	drop proc sp_AddHoliday
go
create procedure sp_AddHoliday
	@reason nvarchar(255), @startedDate datetime, @endDate datetime, @empID int
as
begin
	Declare @restdays int;

	SET @restdays = DATEDIFF(DAY, @startedDate,@endDate);


	INSERT INTO HOLIDAY(reason, restDays,startedDate,endDate,empID)
	VALUES(@reason, @restdays,@startedDate,@endDate,@empID)
end
GO

--! DELETE
if(OBJECT_ID('sp_DeleteHoliday') is not null)
	drop proc sp_DeleteHoliday
go
create procedure sp_DeleteHoliday
	@holidayID int
as
begin
	delete from HOLIDAY 
	WHERE holidayID = @holidayID
END
GO

--! UPDATE
if(OBJECT_ID('sp_UpdateHoliday') is not null)
	drop proc sp_UpdateHoliday
go
create procedure sp_UpdateHoliday
	@holidayID int, @reason nvarchar(255),
	@startedDate datetime, @endDate datetime,
	@empID int
as
begin
	Declare @restdays int;

	SET @restdays = DATEDIFF(DAY, @startedDate,@endDate);


	UPDATE HOLIDAY SET reason = @reason,
						restDays = @restdays, 
						startedDate = @startedDate, 
						endDate = @endDate,
						empID = @empID
	WHERE holidayID = @holidayID
end
go


-- ======================= HOLIDAY ======================= 
--ADD

if(OBJECT_ID('sp_AddWorkTime') is not null)
	drop proc sp_AddWorkTime
go
create procedure sp_AddWorkTime
	@startAt datetime , @stopAt datetime, @empID int
as
begin
	Declare @hours int;

	SET @hours = DATEDIFF(HOUR, @startAt,@stopAt);

	INSERT INTO WORK_TIME(startWorkAt,stopWorkAt,workHour,empID)
	VALUES(@startAt, @stopAt,@hours,@empID)
end
GO

--! DELETE
if(OBJECT_ID('sp_DeleteWorkTime') is not null)
	drop proc sp_DeleteWorkTime
go
create procedure sp_DeleteWorkTime
	@workId int
as
begin
	delete from WORK_TIME 
	WHERE workTimeID = @workId
END
GO

--! UPDATE
if(OBJECT_ID('sp_UpdateWorkTime') is not null)
	drop proc sp_UpdateWorkTime
go
create procedure sp_UpdateWorkTime
	@workId int, @startAt datetime,
	@stopAt datetime, @createdDate datetime,
	@empID int
as
begin
	Declare @hours int;

	SET @hours = DATEDIFF(HOUR, @startAt,@stopAt);


	UPDATE WORK_TIME SET startWorkAt = @startAt,
						stopWorkAt = @stopAt,
						workHour = @hours,
						createdDate = @createdDate,
						empID = @empID
	WHERE workTimeID = @workId
end
go

-- ============== trigger ===================
if(OBJECT_ID('trg_Employee') is not null)
	drop trigger trg_Employee
go
create trigger trg_Employee on EMPLOYEE
for insert,update,delete
As
Begin
	DECLARE @Action as char(1);
	DECLARE @benefitId as int;
    SET @Action = (CASE WHEN EXISTS(SELECT * FROM INSERTED)
                         AND EXISTS(SELECT * FROM DELETED)
                        THEN 'U'  -- Set Action to Updated.
                        WHEN EXISTS(SELECT * FROM INSERTED)
                        THEN 'I'  -- Set Action to Insert.
                        WHEN EXISTS(SELECT * FROM DELETED)
                        THEN 'D'  -- Set Action to Deleted.
                        ELSE NULL -- Skip. It may have been a "failed delete".   
                    END)
	IF @Action = 'U' OR @Action = 'I'
	BEGIN
		IF exists (select benefitID from inserted)
		BEGIN
			select @benefitId = benefitID from inserted;
			update BENEFIT
			set enrollNumber = enrollNumber + 1
			where benefitID = @benefitId;
		END
	END
	
	IF @Action = 'D'
	BEGIN
		IF exists (select benefitID from deleted)
		BEGIN
			select @benefitId = benefitID from deleted;
			update BENEFIT
			set enrollNumber = enrollNumber - 1
			where benefitID = @benefitId;
		END
	END
End
go


-- ============= VIEW ================
if(OBJECT_ID('view_Employee') is not null)
	drop view view_Employee
go
CREATE VIEW view_Employee 
AS
	SELECT emp.*, pos.positionName, dept.deptName, benef.benefitName
	FROM EMPLOYEE emp INNER JOIN POSITION pos
	ON emp.positionID = pos.positionID
	INNER JOIN DEPARTMENT dept
	ON emp.deptID = dept.deptID
	INNER JOIN BENEFIT benef
	ON emp.benefitID = benef.benefitID
GO

