using AutoMapper;
using EMPMGT.Model.DTO;
using EMPMGT.Model.Tentant;
using EMPMGT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMPMGT.Pages
{
    public class EmployeeAddModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        [BindProperty]
        public EmployeeRequestModel m { get; set; }

        public EmployeeAddModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public void OnGet()
        {
            this.m = new EmployeeRequestModel() { CreationDate = DateTime.UtcNow, IsDeleted = false, DateOfBirth = DateTime.UtcNow.AddYears(-18) };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (m.Id == 0)
                {
                    _employeeService.Create(m);
                    return RedirectToPage("/EmployeeGrid");
                }
                else
                {
                    _employeeService.Update(m.Id, m);
                    return RedirectToPage("/EmployeeGrid");
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return RedirectToPage("EmployeeGrid");
            }

        }

        public async Task<IActionResult> OnGetEditAsync(int id)
        {
            try
            {
                Employee o = new Employee();

                o = _employeeService.GetById(id);

                var map = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeRequestModel>());
                var mappingObject = new Mapper(map);

                m = mappingObject.Map<EmployeeRequestModel>(o);


            }
            catch (Exception e)
            {
                Console.Write(e.Message);

            }
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            try
            {

                _employeeService.Delete(id);

            }
            catch (Exception e)
            {
                Console.Write(e.Message);

            }
            return new JsonResult(true);
        }
    }
}
