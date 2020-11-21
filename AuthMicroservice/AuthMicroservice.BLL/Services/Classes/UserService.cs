using AuthMicroservice.BLL.Models.DTO.User;
using AuthMicroservice.BLL.Services.Interfaces;
using AuthMicroservice.DAL.Repositories.SQLServer.Interfaces;
using AutoMapper;
using Microservice.Core.Infrastructure.OperationResult;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthMicroservice.BLL.Services.Classes
{
    public class UserService: IUserService
    {
        private readonly ISQLUnitOfWork _sqlUnitOfWork;
        private readonly IMapper _mapper;

        public UserService(ISQLUnitOfWork sqlUnitOfWork, IMapper mapper)
        {
            _sqlUnitOfWork = sqlUnitOfWork;
            _mapper = mapper;
        }

        public OperationResult<List<UserDTO>> GetUsers()
        {
            var userRepository = _sqlUnitOfWork.GetRepository<IUserSQLServerRepository>();
            var usersResult = userRepository.GetUsersWithRole();

            var result = new OperationResult<List<UserDTO>>
            {
                Data = _mapper.Map<List<UserDTO>>(usersResult.Data),
                Errors = usersResult.Errors,
                Type = usersResult.Type
            };

            return result;
        }
    }
}
