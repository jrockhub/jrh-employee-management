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
    public class UserGridModel : PageModel
    {
        private readonly IUserService _userService;

        public UserPageViewModel m = new UserPageViewModel();
        private readonly ILogger<UserGridModel> _logger;
        public UserGridModel(ILogger<UserGridModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        public void OnGet()
        {
            this.m = new UserPageViewModel();


        }

        public async Task<IActionResult> OnPostAsync([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                var results = await _userService.SearchUsersAsync(param);

                return new JsonResult(new JqueryDataTablesResult<UserPageViewModel>
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
