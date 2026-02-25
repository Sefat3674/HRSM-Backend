select * from Users


create Table PayrollPeriod(
	 PayrollPeriodId INT IDENTITY(1,1) PRIMARY KEY,

    -- Payroll Identification
    PayrollCode VARCHAR(20) NOT NULL,  -- Example: PR-2026-03
    Month INT NOT NULL CHECK (Month BETWEEN 1 AND 12),
    Year INT NOT NULL,

    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,

    -- Payroll Status Control
    Status VARCHAR(20) NOT NULL DEFAULT 'Draft',
    /*
        Draft
        Processing
        Completed
        Cancelled
        Approved (Optional if approval workflow exists)
    */

    -- Summary Information (Filled After Run)
    TotalEmployees INT NULL,
    TotalBasicAmount DECIMAL(18,2) NULL,
    TotalBonusAmount DECIMAL(18,2) NULL,
    TotalDeductionAmount DECIMAL(18,2) NULL,
    TotalNetSalaryAmount DECIMAL(18,2) NULL,

    -- Audit Information
    CreatedBy INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    UpdatedBy INT NULL,
    UpdatedAt DATETIME NULL,

    ProcessedBy INT NULL,
    ProcessedAt DATETIME NULL,

    ApprovedBy INT NULL,
    ApprovedAt DATETIME NULL,

    IsLocked BIT NOT NULL DEFAULT 0,

    -- Prevent Duplicate Payroll For Same Month
    CONSTRAINT UQ_PayrollPeriod UNIQUE (Month, Year)

)

select * from PayrollPeriod

