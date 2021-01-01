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
        public HttpResponseMessage Get()
        {
            using (EmployeeDBContext dbContext = new EmployeeDBContext())
            {
                var Employees = dbContext.Employees.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, Employees);
            }
        }
        public HttpResponseMessage Get(int id)
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
    }
}
