select * from Roles

select *from Users
select * from SalaryStructure

create table SalaryStructure(
   SalaryStructureId  INT Identity(1,1) Primary key,
   UserId Int Not Null,
   BasicSalary Decimal(18,2) not null default 0,
   HouseRentAllowance Decimal(18,2) not null default 0,
   MedicalAllowance Decimal(18,2) not null default 0,
   TransportAllowance Decimal(18,2) not null default 0,
   otherAllowance Decimal(18,2) not null default 0,

   EffectiveFrom Date not null,
   EffectiveTo Date null,

   IsActive Bit not null default 1,

   Constraint FK_SalaryStruvture_Users
		Foreign key(UserId) References Users(UserId)
		

   )

   ALTER TABLE SalaryStructure
ADD CreatedAt DATETIME2(0) NOT NULL DEFAULT SYSDATETIME();





