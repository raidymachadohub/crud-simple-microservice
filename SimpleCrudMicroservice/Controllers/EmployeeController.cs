using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SimpleCrudMicroservice.Domain.Entity;
using SimpleCrudMicroservice.DTO;
using SimpleCrudMicroservice.Mapper;
using SimpleCrudMicroservice.Service.Interfaces;

namespace SimpleCrudMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // private readonly IEmployee _employee;
        //
        // public EmployeeController(IEmployee employee)
        // {
        //     _employee = employee;
        // }

        private void PublishToMessageQueue(string integrationEvent, string eventData)
        {
            // TOOO: Reuse and close connections and channel, etc, 
            var factory = new ConnectionFactory();
            factory.UserName = "admin";
            factory.Password = "admin";
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);
            channel.BasicPublish(exchange: "employee",
                routingKey: integrationEvent,
                basicProperties: null,
                body: body);
        }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
        // {
        //     var result = await _employee.GetAll();
        //     return result.Select(x => new EmployeeDTO(x)).ToList();
        // }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        // {
        //     var employee = await _employee.GetById(id);
        //
        //
        //     if (employee == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return new EmployeeDTO(employee);
        // }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(EmployeeDTO employee)
        {
            try
            {
                //await _employee.Put(EmployeeMap.Mapper(employee));

                var integrationEventData = JsonConvert.SerializeObject(new
                {
                    id = employee.id,
                    newname = employee.name
                });
                PublishToMessageQueue("employee.update", integrationEventData);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> PostEmployee(EmployeeDTO employee)
        {
            //var result = await _employee.Post(EmployeeMap.Mapper(employee));

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = employee.id,
                name = employee.name
            });
            PublishToMessageQueue("employee.add", integrationEventData);

            return new EmployeeDTO(new Employee());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeDTO>> DeleteEmployee(int id)
        {
            // var employee = await _employee.Delete(id);
            //
            // if (employee == null)
            // {
            //     return NotFound();
            // }

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = id
            });
            PublishToMessageQueue("employee.delete", integrationEventData);


            return new EmployeeDTO(new Employee());
        }
    }
}