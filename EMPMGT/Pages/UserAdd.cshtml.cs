using AutoMapper;
using EMPMGT.Model.DTO;
using EMPMGT.Model.Tentant;
using EMPMGT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMPMGT.Pages
{
	public class UserAddModel : PageModel
	{
		private readonly IUserService _userService;
		[BindProperty]
		public UserRequestModel m { get; set; }

		public UserAddModel(IUserService userService)
		{
			_userService = userService;
		}
		public void OnGet()
		{
			this.m = new UserRequestModel() { CreationDate = DateTime.UtcNow, IsDeleted = false };
		}

		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				if (m.Id == 0)
				{
					_userService.Create(m);
					return RedirectToPage("/UserGrid");
				}
				else
				{
					_userService.Update(m.Id, m);
					return RedirectToPage("/UserGrid");
				}
			}
			catch (Exception e)
			{
				Console.Write(e.Message);
				return RedirectToPage("UserGrid");
			}

		}

		public async Task<IActionResult> OnGetEditAsync(int id)
		{
			try
			{
				User o = new User();

				o = _userService.GetById(id);

				var map = new MapperConfiguration(cfg => cfg.CreateMap<User, UserRequestModel>());
				var mappingObject = new Mapper(map);

				m = mappingObject.Map<UserRequestModel>(o);


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

				_userService.Delete(id);

			}
			catch (Exception e)
			{
				Console.Write(e.Message);

			}
			return new JsonResult(true);
		}
	}
}
