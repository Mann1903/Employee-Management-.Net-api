using EmployeeCrud_API.Data;
using EmployeeCrud_API.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCrud_API.Repo
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeRepo(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<List<EmployeeModel>> GetAllAsync()
        {
            var emp = await _context.Employees.Select(x => new EmployeeModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                ContactNumber = x.ContactNumber,
                Address = x.Address,
                Department = x.Department,
                Gender = x.Gender,
                Position = x.Position,
                IsActive = x.IsActive,
                ImageName = x.ImageName
            }).ToListAsync();
            return emp;
        }
        public async Task<EmployeeModel> GetByIdAsync(int Id)
        {
            var emp = await _context.Employees.Where(x => x.Id == Id).Select(x => new EmployeeModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                ContactNumber = x.ContactNumber,
                Address = x.Address,
                Department = x.Department,
                Gender = x.Gender,
                Position = x.Position,
                IsActive = x.IsActive,
                ImageName = x.ImageName
            }).FirstOrDefaultAsync();
            return emp;
        }
        public async Task<int> GetTotalCountAsync()
        {
            var empCount=_context.Employees.Count();
            return empCount;
        }
        public async Task<int> GetCountAsync(string position)
        {
            return _context.Employees.Count(e => e.Position.ToLower() == position.ToLower());
        }
        public async Task<int> CreateEmployeeAsync(EmployeeModel employeeModel)
        {
            var employee = new Employee()
            {
                FirstName = employeeModel.FirstName,
                LastName = employeeModel.LastName,
                Email = employeeModel.Email,
                ContactNumber = employeeModel.ContactNumber,
                Address = employeeModel.Address,
                Department = employeeModel.Department,
                Gender = employeeModel.Gender,
                Position = employeeModel.Position,
                IsActive = employeeModel.IsActive,
                ImageName = employeeModel.ImageName
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee.Id;
        }
        public async Task<int> UpdateEmployeeAsync(int Id, EmployeeModel employeeModel)
        {
            var employee = new Employee()
            {
                Id = Id,
                FirstName = employeeModel.FirstName,
                LastName = employeeModel.LastName,
                Email = employeeModel.Email,
                ContactNumber = employeeModel.ContactNumber,
                Address = employeeModel.Address,
                Department = employeeModel.Department,
                Gender = employeeModel.Gender,
                Position = employeeModel.Position,
                IsActive = employeeModel.IsActive,
                ImageName = employeeModel.ImageName
            };
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee.Id;
        }
        public async Task DeleteEmployeeAsync(int Id)
        {
            var employee = new Employee() { Id = Id };
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
        public async Task<string> UploadFiles(IFormFile files)
        {
            if (files == null || files.Length == 0)
            {
                throw new ArgumentNullException(nameof(files), "File is null or empty");
            }
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Profile_Images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var imageName = files.FileName.Split('.');
            var uniqueFileName = $"{imageName[0]}_{DateTime.Now.Ticks}.{imageName[1]}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await files.CopyToAsync(stream);
            }
            return uniqueFileName;
        }
        public async Task UpdateFile(int id, string fileName)
        {
            if (fileName != null)
            {
                var emp = await _context.Employees.FindAsync(id);
                {
                    emp.Id = id;
                    emp.ImageName = fileName;
                };
                _context.Employees.Update(emp);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Employee not found");
            }
        }
    }
}
