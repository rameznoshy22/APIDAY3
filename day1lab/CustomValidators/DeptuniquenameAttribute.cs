using day1lab.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;


namespace day1lab.CustomValidators
{
    public class DeptuniquenameAttribute :ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            APIcontext dbContext = (APIcontext)validationContext.GetService(typeof(APIcontext));
            var department = (Department)validationContext.ObjectInstance;

            // Check if another department with the same name exists (excluding current department)
            Department existingDepartment = dbContext.Departments
          .FirstOrDefault(d => d.Name == value.ToString() && d.Id != department.Id);

            if (existingDepartment != null)
            {
                return new ValidationResult("Department name must be unique.");
            }

            return ValidationResult.Success;

            //var name = value as string;

            //// 1. Check if empty
            //if (string.IsNullOrWhiteSpace(name))
            //    return new ValidationResult("Department name is required");

            //// 2. Check length
            //if (name.Length > 50)
            //    return new ValidationResult("Name cannot exceed 50 characters");

            //// 3. Regex pattern (letters, spaces, hyphens only)
            //if (!Regex.IsMatch(name, @"^[a-zA-Z\s\-]+$"))
            //    return new ValidationResult("Only letters, spaces and hyphens allowed");

            //// 4. Block reserved words
            //var reservedWords = new[] { "admin", "system", "root" };
            //if (reservedWords.Contains(name.ToLower()))
            //    return new ValidationResult("This name is reserved");

            //return ValidationResult.Success;
        }
    }

}

