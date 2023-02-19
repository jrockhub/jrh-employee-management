using EMPMGT.Model.Tentant;
using EMPMGT.Model;
using AutoMapper;
using EMPMGT.DataContext;
using EMPMGT.Helper;
using EMPMGT.Model.DTO;
using AutoMapper.QueryableExtensions;
using EMPMGT.Model.PageViewModel;
using Jrockhub.DataTablesServerSide.Infrastructure;
using Jrockhub.DataTablesServerSide.Models;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;

namespace EMPMGT.Services
{
	public interface IEmployeeService
	{
		IEnumerable<Employee> GetAll();
		Employee GetById(int id);
        Employee Create(EmployeeRequestModel m);
        Employee Update(int id, EmployeeRequestModel m);
		bool Delete(int id);

		Task<JqueryDataTablesPagedResults<EmployeePageViewModel>> SearchEmployeesAsync(JqueryDataTablesParameters table);

    }
	public class EmployeeService : IEmployeeService
	{
		private EmpContext _empContext;
		private readonly AutoMapper.IConfigurationProvider _mappingConfiguration;


		public EmployeeService(EmpContext empContext, AutoMapper.IConfigurationProvider mappingConfiguration)
		{
			_empContext = empContext;
			_mappingConfiguration = mappingConfiguration;
		}
		public IEnumerable<Employee> GetAll()
		{
			return _empContext.Employees.Where(x => !x.IsDeleted);
		}

		public Employee GetById(int id)
		{
			var employee = _empContext.Employees.Find(id);
			if (employee == null) throw new KeyNotFoundException("Employee not found.");

			return employee;
		}

		public Employee Create(EmployeeRequestModel m)
		{
			// Validate the email if the email already exist into our system.
			if (_empContext.Employees.Any(x => x.Email == m.Email))
				throw new AppException("Employee with the email '" + m.Email + "' already exists.");


			var map = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeRequestModel, Employee>());
			var mappingObject = new Mapper(map);
			var employee = mappingObject.Map<Employee>(m);


			// Save into Database

			_empContext.Employees.Add(employee);
			_empContext.SaveChanges();
			return employee;
		}

		public Employee Update(int id, EmployeeRequestModel m)
		{
			var emp = _empContext.Employees.AsNoTracking().FirstOrDefault(x=> x.Id == id);
			if (emp == null) throw new KeyNotFoundException("Employee not found.");

			//Validate the email if email already exist into system execpt same employee.
			if (m.Email != emp.Email && _empContext.Employees.Any(x => x.Email == m.Email))
				throw new AppException("Employee with the email '" + m.Email + "' already exists.");

			// model copy to employee and save
			var map = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeRequestModel, Employee>());
			var mappingObject = new Mapper(map);
			var employee = mappingObject.Map<Employee>(m);
			employee.Id = id;

			
			_empContext.Set<Employee>().Update(employee);
			_empContext.SaveChanges();

            return employee;
        }

		public bool Delete(int id)
		{
			bool deleteStatus = false;
			var employee = _empContext.Employees.Find(id);
			if (employee == null) throw new KeyNotFoundException("Employee not found.");
			{
				employee.IsDeleted = true;
				_empContext.Update(employee);
				_empContext.SaveChanges();
				deleteStatus = true;

			}
			return deleteStatus;

        }

		// For Filter Search Grid
		public async Task<JqueryDataTablesPagedResults<EmployeePageViewModel>> SearchEmployeesAsync(JqueryDataTablesParameters table)
		{

			EmployeePageViewModel[] items = null;


			IQueryable<EmployeePageViewModel> query = (from u in _empContext.Employees

													   where !u.IsDeleted
													   select new EmployeePageViewModel
													   {
														   Id = u.Id,
														   Email = u.Email,
														   FirstName = u.FirstName,
														   LastName = u.LastName,
														   DateOfBirthFormatted = u.DateOfBirth.ToString("MM-dd-yyyy"),
														   Department = u.Department,
														   IsActive = u.IsActive,
														   IsActiveFormatted = u.IsActive ? "Active" : "In-Active",


													   });

			var adList = table.AdditionalValues.ToList();

			if (adList[0] != "" && !string.IsNullOrWhiteSpace(adList[0]))
			{
				var email = adList[0];
				query = query.Where(x => x.Email == email);
			}
			if (adList[1] != "" && !string.IsNullOrWhiteSpace(adList[1]))
			{
				var isActive = Convert.ToBoolean(adList[1]);
				query = query.Where(x => x.IsActive == isActive);
			}



			query = SearchOptionsProcessor<EmployeePageViewModel, EmployeePageViewModel>.Apply(query, table.Columns);
			query = SortOptionsProcessor<EmployeePageViewModel, EmployeePageViewModel>.Apply(query, table);

			int size = await query.CountAsync();

			if (table.Length > 0)
			{
				items = await query
				.Skip((table.Start / table.Length) * table.Length)
				.Take(table.Length)
				.ProjectTo<EmployeePageViewModel>(_mappingConfiguration)
				.ToArrayAsync();
			}
			else
			{
				items = await query.ProjectTo<EmployeePageViewModel>(_mappingConfiguration).ToArrayAsync();
			}

			return new JqueryDataTablesPagedResults<EmployeePageViewModel>
			{
				Items = items,
				TotalSize = size
			};
		}
	}
}
