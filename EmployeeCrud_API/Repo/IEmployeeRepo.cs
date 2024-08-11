using EmployeeCrud_API.Model;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeCrud_API.Repo
{
    public interface IEmployeeRepo
    {
        Task<List<EmployeeModel>> GetAllAsync();
        Task<EmployeeModel> GetByIdAsync(int Id);
        Task<int> GetTotalCountAsync();
        Task<int> GetCountAsync(string position);
        Task<int> CreateEmployeeAsync(EmployeeModel employeeModel);
        Task<int> UpdateEmployeeAsync(int id,EmployeeModel emplModel);
        Task DeleteEmployeeAsync(int Id);
        Task<string> UploadFiles(IFormFile files);
        Task UpdateFile(int id,string file);
    }
}
