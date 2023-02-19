using AutoMapper;
using AutoMapper.QueryableExtensions;
using EMPMGT.DataContext;
using EMPMGT.Helper;
using EMPMGT.Model;
using EMPMGT.Model.DTO;
using EMPMGT.Model.PageViewModel;
using EMPMGT.Model.Tentant;
using Jrockhub.DataTablesServerSide.Infrastructure;
using Jrockhub.DataTablesServerSide.Models;
using Microsoft.EntityFrameworkCore;

namespace EMPMGT.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Create(UserRequestModel m);
        void Update(int id, UserRequestModel m);
        void Delete(int id);

        Task<JqueryDataTablesPagedResults<UserPageViewModel>> SearchUsersAsync(JqueryDataTablesParameters table);
    }
    public class UserService : IUserService
    {
        private EmpContext _empContext;
        private readonly AutoMapper.IConfigurationProvider _mappingConfiguration;

        public UserService(EmpContext empContext, AutoMapper.IConfigurationProvider mappingConfiguration)
        {
            _empContext = empContext;
            _mappingConfiguration = mappingConfiguration;
        }

        public IEnumerable<User> GetAll()
        {
            return _empContext.Users.Where(x => !x.IsDeleted);
        }

        public User GetById(int id)
        {
            var user = _empContext.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found.");

            return user;
        }

        public void Create(UserRequestModel m)
        {
            // Validate the email if the email already exist into our system.
            if (_empContext.Users.Any(x => x.Email == m.Email))
                throw new AppException("User with the email '" + m.Email + "' already exists.");

            var map = new MapperConfiguration(cfg => cfg.CreateMap<UserRequestModel, User>());
            var mappingObject = new Mapper(map);
            var user = mappingObject.Map<User>(m);



            // if we need to create hash password or encrypted then we can use Cryptography with use of Hash and Salt Key. 
            // Currently we are not using the encription mode.

            user.Password = m.Password;

            // Save into Database

            _empContext.Users.Add(user);
            _empContext.SaveChanges();

        }

        public void Update(int id, UserRequestModel m)
        {
            var use = _empContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (use == null) throw new KeyNotFoundException("Employee not found.");

            //Validate the email if email already exist into system execpt same user.
            if (m.Email != use.Email && _empContext.Users.Any(x => x.Email == m.Email))
                throw new AppException("User with the email '" + m.Email + "' already exists.");


            // model copy to user and save
            var map = new MapperConfiguration(cfg => cfg.CreateMap<UserRequestModel, User>());
            var mappingObject = new Mapper(map);
            var user = mappingObject.Map<User>(m);
            user.Id = id;

            _empContext.Set<User>().Update(user);
            _empContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _empContext.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found.");
            {
                user.IsDeleted = true;
                _empContext.Update(user);
                _empContext.SaveChanges();
            }
        }

        // For Filter Search Grid
        public async Task<JqueryDataTablesPagedResults<UserPageViewModel>> SearchUsersAsync(JqueryDataTablesParameters table)
        {

            UserPageViewModel[] items = null;


            IQueryable<UserPageViewModel> query = (from u in _empContext.Users

                                                   where !u.IsDeleted
                                                   select new UserPageViewModel
                                                   {
                                                       Id = u.Id,
                                                       Email = u.Email,
                                                       FirstName = u.FirstName,
                                                       LastName = u.LastName,
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



            query = SearchOptionsProcessor<UserPageViewModel, UserPageViewModel>.Apply(query, table.Columns);
            query = SortOptionsProcessor<UserPageViewModel, UserPageViewModel>.Apply(query, table);

            int size = await query.CountAsync();

            if (table.Length > 0)
            {
                items = await query
                .Skip((table.Start / table.Length) * table.Length)
                .Take(table.Length)
                .ProjectTo<UserPageViewModel>(_mappingConfiguration)
                .ToArrayAsync();
            }
            else
            {
                items = await query.ProjectTo<UserPageViewModel>(_mappingConfiguration).ToArrayAsync();
            }

            return new JqueryDataTablesPagedResults<UserPageViewModel>
            {
                Items = items,
                TotalSize = size
            };
        }
    }
}
