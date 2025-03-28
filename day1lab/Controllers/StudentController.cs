using Azure;
using day1lab.DTO;
using day1lab.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace day1lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
         private readonly APIcontext context;

        public StudentController(APIcontext context)
        {
            this.context = context;
        }



        // GET: api/<StudentController>
        [HttpGet]
        public IActionResult Getallstudent()
        {
       
            List<Student> students= context.Students.Include(s=>s.Department).ToList();
            if (students.Count==0)
            {
                return NotFound("No students found");
            }
            List<StudentoneDTO>studentdto= new List<StudentoneDTO>();
           
            for (int i = 0; i < students.Count; i++)
            {
                StudentoneDTO student = new StudentoneDTO();
                student.Id = students[i].Id;
                student.Name = students[i].Name;
                student.Age = students[i].Age;
                student.Address = students[i].Address;
                student.DOB = students[i].DOB;
                student.DepartmentName = students[i].Department.Name;
                studentdto.Add(student);
            }
            return Ok(studentdto);    
        }

        //GET api/<StudentController>/5
        [HttpGet("{id:int}")]
        public IActionResult Getbyid(int id)
        {
            Student? student = context.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound($"No Student with id : {id}");
            }
            StudentoneDTO studentdto = new StudentoneDTO();
            studentdto.Id = student.Id;
            studentdto.Name = student.Name;
            studentdto.Age = student.Age;
            studentdto.Address = student.Address;
            studentdto.DOB = student.DOB;
            studentdto.DepartmentName = student.Department.Name;
            return Ok(studentdto);
          
        }

        [HttpPost]
        public IActionResult Addstudent([FromBody] StudentputDTO _student)
        {
            if(ModelState.IsValid)
            {
                Student student = new Student();
                student.Name = _student.Name;
                student.Age = _student.Age;
                student.Address = _student.Address;
                student.DOB = _student.DOB;
                student.DeptID = _student.DeptID;
                context.Students.Add(student);
                context.SaveChanges();
                //---------------
                //question will i do this every time????????????????????????????????????????????
                //and how can i display a StudentoneDTO without return to the DB????????????????????????????????
                //Student? student1 = context.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
                //StudentoneDTO? studentdto = new StudentoneDTO();
                //studentdto.Id = student.Id;
                //studentdto.Name = student.Name;
                //studentdto.Age = student.Age;
                //studentdto.Address = student.Address;
                //studentdto.DOB = student.DOB;
                //studentdto.DepartmentName = student.Department.Name;

                // return CreatedAtAction(nameof(Getbyid), new { id = student.Id }, studentdto);
                return CreatedAtAction(nameof(Getbyid), new { id = student.Id }, student);
            }
            return BadRequest(ModelState);
        }
        // it works but give exception???????????????????????????????????????????????????????
        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public IActionResult PutStudentDetails(int id, [FromBody] StudentputDTO _student)
        {
            if (ModelState.IsValid)
            {

                Student? student = context.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
                if (student == null)
                {
                    return NotFound($"No students found matching id: {id} to be modified");
                }
                // another exception when active this!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //student.Id=id;
                student.Name = _student.Name;
                student.Age = _student.Age ;
                student.Address = _student.Address;
                student.DOB = _student.DOB;
                student.DeptID = _student.DeptID;
                context.SaveChanges();

               // return Ok(student);
               //will both work??????????????????????????????????????????????????????????????????????????
              //return StatusCode(204, _student);
              return Ok( _student);
            }
            return BadRequest(ModelState);
        }
        [HttpPatch("{id}")]
        public IActionResult PatchStudentDetails([FromRoute]int id, [FromBody] JsonPatchDocument<Student> patchofstudentdocument)
        {
            if (ModelState.IsValid)
            {
                Student? student = context.Students.FirstOrDefault(s => s.Id == id);
                if (student == null)
                {
                    return NotFound($"No students found matching id: {id} to be modified");
                }
                patchofstudentdocument.ApplyTo(student);
                context.SaveChanges();
                return Ok(student);

            }
            return BadRequest(ModelState);
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           
            if (ModelState.IsValid)
            {
                Student? student = context.Students.FirstOrDefault(s => s.Id == id);
                if (student == null)
                {
                    return NotFound($"No students found matching id: {id} to be removed");
                }
                try
                {
                    context.Students.Remove(student);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                // return Ok(student);
                return StatusCode(204,student);
            }
            return BadRequest(ModelState);
        }
    }
}
