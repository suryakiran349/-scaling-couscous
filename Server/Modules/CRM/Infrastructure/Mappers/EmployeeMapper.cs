﻿using ComposedHealthBase.Server.Mappers;
using Server.Modules.CRM.Entities;
using Shared.DTOs.CRM;

public class EmployeeMapper : IMapper<Employee, EmployeeDto>
{
    public EmployeeDto Map(Employee entity)
    {
        return new EmployeeDto
        {
            Id = entity.Id,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            LastModifiedBy = entity.LastModifiedBy,
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            DOB = entity.DOB,
            Address1 = entity.Address1,
            Address2 = entity.Address2!,
            Address3 = entity.Address3!,
            Postcode = entity.Postcode,
            Email = entity.Email,
            Telephone = entity.Telephone,
            CustomerId = entity.CustomerId,
            JobRole = entity.JobRole,
            Department = entity.Department,
            LineManager = entity.LineManager
        };
    }

    public Employee Map(EmployeeDto dto)
    {
        return new Employee
        {
            Id = dto.Id,
            IsActive = dto.IsActive,
            CreatedBy = dto.CreatedBy,
            LastModifiedBy = dto.LastModifiedBy,
            CreatedDate = dto.CreatedDate,
            ModifiedDate = dto.ModifiedDate,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DOB = dto.DOB,
            Address1 = dto.Address1,
            Address2 = dto.Address2,
            Address3 = dto.Address3,
            Postcode = dto.Postcode,
            Email = dto.Email,
            Telephone = dto.Telephone,
            CustomerId = dto.CustomerId,
            JobRole = dto.JobRole,
            Department = dto.Department,
            LineManager = dto.LineManager
        };
    }

    public IEnumerable<EmployeeDto> Map(IEnumerable<Employee> entities)
    {
        return entities.Select(Map);
    }

    public IEnumerable<Employee> Map(IEnumerable<EmployeeDto> dtos)
    {
        return dtos.Select(Map);
    }

    public void Map(EmployeeDto dto, Employee entity)
    {
        entity.Id = dto.Id;
        entity.IsActive = dto.IsActive;
        entity.CreatedBy = dto.CreatedBy;
        entity.LastModifiedBy = dto.LastModifiedBy;
        entity.CreatedDate = dto.CreatedDate;
        entity.ModifiedDate = dto.ModifiedDate;
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.DOB = dto.DOB;
        entity.Address1 = dto.Address1;
        entity.Address2 = dto.Address2;
        entity.Address3 = dto.Address3;
        entity.Postcode = dto.Postcode;
        entity.Email = dto.Email;
        entity.Telephone = dto.Telephone;
        entity.CustomerId = dto.CustomerId;
        entity.JobRole = dto.JobRole;
        entity.Department = dto.Department;
        entity.LineManager = dto.LineManager;
    }

    public void Map(Employee entity, EmployeeDto dto)
    {
        dto.Id = entity.Id;
        dto.IsActive = entity.IsActive;
        dto.CreatedBy = entity.CreatedBy;
        dto.LastModifiedBy = entity.LastModifiedBy;
        dto.CreatedDate = entity.CreatedDate;
        dto.ModifiedDate = entity.ModifiedDate;
        dto.FirstName = entity.FirstName;
        dto.LastName = entity.LastName;
        dto.DOB = entity.DOB;
        dto.Address1 = entity.Address1;
        dto.Address2 = entity.Address2!;
        dto.Address3 = entity.Address3!;
        dto.Postcode = entity.Postcode;
        dto.Email = entity.Email;
        dto.Telephone = entity.Telephone;
        dto.CustomerId = entity.CustomerId;
        dto.JobRole = entity.JobRole;
        dto.Department = entity.Department;
        dto.LineManager = entity.LineManager;
    }

    public void Map(IEnumerable<EmployeeDto> dtos, IEnumerable<Employee> entities)
    {
        var dtosArray = dtos.ToArray();
        var entitiesArray = entities.ToArray();
        for (int i = 0; i < Math.Min(dtosArray.Length, entitiesArray.Length); i++)
        {
            Map(dtosArray[i], entitiesArray[i]);
        }
    }

    public void Map(IEnumerable<Employee> entities, IEnumerable<EmployeeDto> dtos)
    {
        var dtosArray = dtos.ToArray();
        var entitiesArray = entities.ToArray();
        for (int i = 0; i < Math.Min(dtosArray.Length, entitiesArray.Length); i++)
        {
            Map(entitiesArray[i], dtosArray[i]);
        }
    }
}