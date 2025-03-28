using day1lab.DTO;
using day1lab.Models;
using DemoAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace day1lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly APIcontext context;

        public DepartmentController(APIcontext context)
        {
            this.context = context;
        }
        //api/DEpartment
        [HttpGet]

        public IActionResult GetAllDEpartmet()
        {
            List<Department> deptList = context.Departments.Include(d=>d.Students).ToList();
            List<DeptoneDTO> deptoneDTOs = new List<DeptoneDTO>();
            
            for(int i=0;i < deptList.Count; i++)
            {
                DeptoneDTO DepDto = new DeptoneDTO();
                DepDto.Id = deptList[i].Id;
                DepDto.Name = deptList[i].Name;
                DepDto.Manager = deptList[i].Manager;
                DepDto.location = deptList[i].location;
                foreach (var item in deptList[i].Students)
                {
                    DepDto.StudentsName.Add(item.Name);
                }
                deptoneDTOs.Add(DepDto);
            }

            return Ok(deptoneDTOs);
        }

        [HttpGet("{id:int}")]//api/DEpartment/1
        [AllowAnonymous]
        public IActionResult GetByID(int id)
        {
            Department dept = context.Departments.Include(d => d.Students).FirstOrDefault(d => d.Id == id);
            DeptoneDTO DepDto = new DeptoneDTO();
            DepDto.Id = id;
            DepDto.Name = dept.Name;
            DepDto.Manager = dept.Manager;
            DepDto.location = dept.location;
            foreach (var item in dept.Students)
            {
                DepDto.StudentsName.Add(item.Name);
            }
            return Ok(DepDto);

        }


        ////api/Department
        [HttpPost("v1")]//add Resourse "DEpartment
        public IActionResult PostAllDEpartmet(Department Dept)
        {
            if (ModelState.IsValid == true)
            {
            
                context.Departments.Add(Dept);
                context.SaveChanges();
           
                return CreatedAtAction(nameof(GetByID), new { id = Dept.Id }, Dept);
            }
            
            return BadRequest(ModelState);

        }
        [HttpPost("v2")]//add Resourse "DEpartment
        [TestAction]
        public IActionResult PostAllDEpartmet2(Department Dept)
        {
            if (ModelState.IsValid == true)
            {

                context.Departments.Add(Dept);
                context.SaveChanges();

                return CreatedAtAction(nameof(GetByID), new { id = Dept.Id }, Dept);
            }

            return BadRequest(ModelState);

        }
        //api/Department/7

        [HttpPut("{id:int}")]
        public IActionResult Update([FromRoute] int id, [FromBody] Department dept)
        {
            if (ModelState.IsValid == true)
            {
                Department OldDept = context.Departments.FirstOrDefault(d => d.Id == id);
                if (OldDept != null)
                {
                    OldDept.Name = dept.Name;
                    OldDept.Manager = dept.Manager;
                    context.SaveChanges();
                    return StatusCode(204, OldDept);
                }
                return BadRequest("Id Not Valid");

            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id)
        {
            Department OldDept = context.Departments.FirstOrDefault(d => d.Id == id);
            if (OldDept != null)
            {
                try
                {
                    context.Departments.Remove(OldDept);
                    context.SaveChanges();
                    return StatusCode(204, "Record Remove Success");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Id Not Found");
        }
    }


}

