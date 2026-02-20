SELECT 
    u.UserId,
    u.UserName,
   
    s.BasicSalary,
    s.HouseRentAllowance,
    s.MedicalAllowance,
    s.TransportAllowance,
    s.OtherAllowance,
    s.EffectiveFrom,
    s.EffectiveTo,
    s.IsActive
FROM Users AS u
LEFT JOIN SalaryStructure AS s
    ON u.UserId = s.UserId;


