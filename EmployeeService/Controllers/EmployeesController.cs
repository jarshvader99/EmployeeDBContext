using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeService.Models;

namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage LoadAllEmployees()
        {
            using (EmployeeDBContext dbContext = new EmployeeDBContext())
            {
                var Employees = dbContext.Employees.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, Employees);
            }
        }
        [HttpGet]
        public HttpResponseMessage LoadEmployee(int id)
        {
            using (EmployeeDBContext dbContext = new EmployeeDBContext())
            {
                var entity = dbContext.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with ID " + id.ToString() + " not found");
                }
            }
        }
        [HttpGet]
        public HttpResponseMessage LoadEmployeesByGender(string gender)
        {
            using (EmployeeDBContext dbContext = new EmployeeDBContext())
            {
                try
                {
                    if(gender.Equals("all") || gender.Equals("All"))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                        dbContext.Employees.ToList());
                    }
                    else
                    {
                        var Employees = dbContext.Employees.Where(x => x.Gender == gender).ToList();
                        return Request.CreateResponse(HttpStatusCode.OK, Employees);
                    }
                }
                catch
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                        "Value for gender must be Male, Female or All. " + gender + " is invalid.");
                }
            }
        }
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBContext dbContext = new EmployeeDBContext())
                {
                    dbContext.Employees.Add(employee);
                    dbContext.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        employee.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Put([FromBody]int id, [FromUri]Employee employee)
        {
            try
            {
                using (EmployeeDBContext dbContext = new EmployeeDBContext())
                {
                    var entity = dbContext.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBContext dbContext = new EmployeeDBContext())
                {
                    var entity = dbContext.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        dbContext.Employees.Remove(entity);
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
