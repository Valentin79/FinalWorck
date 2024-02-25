using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;
using AutoMapper;
using UserApi.Abstractions;
using UserApi.DB;
using DbApi;




namespace UserApi.Service
{
    public class UserService : IUserService
    {
        private IMapper _mapper;
        private AppDbContext _appDbContext;

        public UserService(IMapper mapper, AppDbContext appDbContext)
        {
            this._mapper = mapper;
            this._appDbContext = appDbContext;
        }
        // доступ к методу должен быть только у админа
        public void AddAdmin(string name, string email, string password)
        {
            using (_appDbContext)
            {
                if (_appDbContext.Users.FirstOrDefault(x => x.Name == name) != null)
                {
                    throw new Exception("Пользователь с таким именем существует");
                }

                if (_appDbContext.Users.FirstOrDefault(x => x.Email == email) != null)
                {
                    throw new Exception("Пользователь с такой почтой существует");
                }
                var user = new User();
                user.Name = name;
                user.Email = email;
                user.RoleId = UserRole.Administrator;
                user.Salt = new byte[16];
                new Random().NextBytes(user.Salt);
                var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
                SHA512 shaM = new SHA512Managed();

                user.Password = shaM.ComputeHash(data);

                _appDbContext.Add(user);
                _appDbContext.SaveChanges();
            }
        }

        public void AddUser(string name, string email, string password)
        {
            using (_appDbContext)
            {
                if (_appDbContext.Users.Count() != 0)
                {

                    if (_appDbContext.Users.FirstOrDefault(x => x.Name == name) != null)
                    {
                        throw new Exception("Пользователь с таким именем существует");
                    }

                    if (_appDbContext.Users.FirstOrDefault(x => x.Email == email) != null)
                    {
                        throw new Exception("Пользователь с такой почтой существует");
                    }
                    var user = new User();
                    user.Name = name;
                    user.Email = email;
                    user.RoleId = UserRole.User;
                    user.Salt = new byte[16];
                    new Random().NextBytes(user.Salt);
                    var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
                    SHA512 shaM = new SHA512Managed();

                    user.Password = shaM.ComputeHash(data);

                    _appDbContext.Add(user);
                    _appDbContext.SaveChanges();
                }
                else // кто первый встал - того и тапки.
                {
                    var user = new User();
                    user.Name = name;
                    user.Email = email;
                    user.RoleId = UserRole.Administrator;
                    user.Salt = new byte[16];
                    new Random().NextBytes(user.Salt);
                    var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
                    SHA512 shaM = new SHA512Managed();

                    user.Password = shaM.ComputeHash(data);

                    _appDbContext.Add(user);
                    _appDbContext.SaveChanges();
                }
            }
        }

        public void DeleteUser(int id)
        {
            using (_appDbContext)
            {
                var user = _appDbContext.Users.FirstOrDefault(x => x.Id == id);
                if (user == null)
                {
                    throw new Exception("Пользователь не существует");
                }
                else
                {
                    _appDbContext.Users.Remove(user);
                    _appDbContext.SaveChanges();
                }
            }
        }

        public UserModel GetUser(int? id, string? email)
        {
            var user = new User();
            var userModel = new UserModel();
            using (_appDbContext)
            {
                var query = _appDbContext.Users;
                if (id.HasValue)
                    query = (DbSet<User>)query.Where(x => x.Id == id);
                if (!string.IsNullOrEmpty(email))
                    query = (DbSet<User>)query.Where(x => x.Email == email);
                user = query.FirstOrDefault();
                if (user == null)
                    return null;
                else
                    userModel = _mapper.Map<UserModel>(user);
                return userModel;
            }
        }

        public List<UserModel> GetUsers()
        {
            var users = new List<UserModel>();
            using (_appDbContext)
            {
                var result = _appDbContext.Users.ToList();
                foreach (var r in result)
                {
                    var user = new UserModel();
                    user = _mapper.Map<UserModel>(r);
                    users.Add(user);
                }
                return users;
            }
        }

        public UserRole UserCheck(string email, string password)
        {
            using (_appDbContext)
            {
                var user = _appDbContext.Users.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    throw new Exception("Пользователь не найден");
                }

                var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
                SHA512 shaM = new SHA512Managed();
                var brassword = shaM.ComputeHash(data);
                if (user.Password.SequenceEqual(brassword))
                {
                    return user.RoleId;
                }
                else
                {
                    throw new Exception("Неверный пароль");
                }
            }
        }
    }
}
