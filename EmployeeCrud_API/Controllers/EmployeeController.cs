using EmployeeCrud_API.Model;
using EmployeeCrud_API.Repo;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmployeeCrud_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo _empRepo;
        private readonly string _imagesFolder;

        public EmployeeController(IEmployeeRepo empRepo)
        {
            _empRepo = empRepo;
            _imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Profile_Images");
        }
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var employee = await _empRepo.GetAllAsync();
            if (employee == null)
            {
                return NoContent();
            }
            return Ok(employee);
        }
        [HttpGet]
        [Route("GetEmp/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var employee = await _empRepo.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpGet]
        [Route("/wwwroot/Profile_Images/{imageName}")]
        public async Task<IActionResult> GetImage([FromRoute] string imageName)
        {
            var imagePath = Path.Combine(_imagesFolder, imageName);
            if (System.IO.File.Exists(imagePath))
            {
                return PhysicalFile(imagePath, "image/png");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("total")]
        public async Task<ActionResult<int>> GetEmployeesCount()
        {
            var empCount=await _empRepo.GetTotalCountAsync();
            return empCount;
        }
        [HttpGet("position/{position}")]
        public async Task<ActionResult<int>> GetCountByPosition(string position)
        {
            var count = await _empRepo.GetCountAsync(position);
            return count;
        }
        [HttpPost]
        [Route("CreateEmp")]
        public async Task<IActionResult> CreateEmployee()
        {
            EmployeeModel emp = JsonConvert.DeserializeObject<EmployeeModel>(Request.Form["data"]);
            var employeeId = await _empRepo.CreateEmployeeAsync(emp);
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files[0];
                var myFile = await _empRepo.UploadFiles(file);
                await _empRepo.UpdateFile(employeeId, myFile);
            }
            return Ok();
        }
        [HttpPut]
        [Route("UpdateEmp/{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute]int id)
        {
            EmployeeModel emp = JsonConvert.DeserializeObject<EmployeeModel>(Request.Form["data"]);
            var employeeId = await _empRepo.UpdateEmployeeAsync(id, emp);
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files[0];
                var myFile = await _empRepo.UploadFiles(file);
                await _empRepo.UpdateFile(employeeId, myFile);
            }
            return Ok();
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            await _empRepo.DeleteEmployeeAsync(id);
            return Ok();
        }
    }
}
