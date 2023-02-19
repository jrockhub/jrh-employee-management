using EMPMGT.Model.DTO;
using EMPMGT.Model;
using EMPMGT.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AutoMapper;
using EMPMGT.Model.Tentant;

namespace EMPMGT.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {

            _employeeService = employeeService;
        }

        [HttpGet("GetAll")]
        public ActionResult<ApiResponse<EmployeeRequestModel>> GetAllEmployee()
        {
            try
            {
                var employee = _employeeService.GetAll().Select(item => new EmployeeRequestModel()
                {
                    Id = item.Id,
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    CreationDate = item.CreationDate,
                    DateOfBirth = item.DateOfBirth,
                    Department = item.Department,
                    IsActive = item.IsActive,
                    IsDeleted = item.IsDeleted
                }).SingleOrDefault();

                if (employee != null)
                {
                    return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Success, ResponseObject = employee });
                }
                else
                {
                    return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Error, Message = "Records not found." });
                }

            }
            catch (Exception e)
            {

                return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Error, Message = "Unable to get employee. Please try after some time." });
            }
        }

        [HttpGet("GetById")]
        public ActionResult<ApiResponse<EmployeeRequestModel>> GetById([BindRequired] int id)
        {
            try
            {
                var result = _employeeService.GetById(id);



                if (result != null)
                {

                    var map = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeRequestModel>());
                    var mappingObject = new Mapper(map);
                    var employee = mappingObject.Map<EmployeeRequestModel>(result);


                    return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Success, ResponseObject = employee });
                }
                else
                {
                    return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Error, Message = "Employee does not exists." });
                }

            }
            catch (Exception e)
            {

                return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Error, Message = "Unable to get employee" });
            }
        }


        [HttpPost("Create")]
        public ActionResult<ApiResponse<EmployeeRequestModel>> Create(EmployeeRequestModel m)
        {
            try
            {

                var result = _employeeService.Create(m);

                if (result != null)
                {

                    var map = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeRequestModel>());
                    var mappingObject = new Mapper(map);
                    var employee = mappingObject.Map<EmployeeRequestModel>(result);


                    return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Success, ResponseObject = employee, Message = "Employee added successfully." });
                }
                else
                {
                    return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Error, Message = "Records not added please try after some time." });
                }

            }
            catch (Exception e)
            {

                return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Error, Message = "Something went wrong, please check the request data." });
            }
        }

        [HttpPost("Update")]
        public ActionResult<ApiResponse<EmployeeRequestModel>> Update([BindRequired] int id, EmployeeRequestModel m)
        {
            try
            {

                var result = _employeeService.Update(id, m);

                if (result != null)
                {

                    var map = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeRequestModel>());
                    var mappingObject = new Mapper(map);
                    var employee = mappingObject.Map<EmployeeRequestModel>(result);


                    return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Success, ResponseObject = employee, Message = "Employee updated successfully." });
                }
                else
                {
                    return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Error, Message = "Records not updated please try after some time." });
                }

            }
            catch (Exception e)
            {

                return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Error, Message = "Something went wrong, please check the request data." });
            }
        }

        [HttpGet("Delete")]
        public ActionResult<ApiResponse<EmployeeRequestModel>> Delete([BindRequired] int id)
        {
            try
            {
                var result = _employeeService.Delete(id);
                if (result == true)
                {
                    return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Success, Message = "Employee deleted successfully." });
                }
                else
                {
                    return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Error, Message = "Sorry, Employee not deleted. " });
                }
            }
            catch (Exception e)
            {

                return Ok(new ApiResponse<EmployeeRequestModel> { ResponseCode = ResponseCodeType.Error, Message = "Something went wrong, please check the request data." });
            }
        }
    }
}
