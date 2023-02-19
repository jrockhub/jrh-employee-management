using AutoMapper;
using EMPMGT.Model.DTO;
using EMPMGT.Model.PageViewModel;
using EMPMGT.Model.Tentant;
using EMPMGT.Services;
using Jrockhub.DataTablesServerSide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMPMGT.Pages
{
    public class EmployeeGridModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        public EmployeePageViewModel m = new EmployeePageViewModel();
        private readonly ILogger<EmployeeGridModel> _logger;
        public EmployeeGridModel(ILogger<EmployeeGridModel> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }
        public void OnGet()
        {
            this.m = new EmployeePageViewModel();


        }

        public async Task<IActionResult> OnPostAsync([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                var results = await _employeeService.SearchEmployeesAsync(param);

                return new JsonResult(new JqueryDataTablesResult<EmployeePageViewModel>
                {
                    Draw = param.Draw,
                    Data = results.Items,
                    RecordsFiltered = results.TotalSize,
                    RecordsTotal = results.TotalSize
                });
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new JsonResult(new { error = "Internal Server Error" });
            }
        }
    }
}
