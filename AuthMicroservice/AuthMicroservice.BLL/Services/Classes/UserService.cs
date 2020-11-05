using AuthMicroservice.BLL.Models.DTO.User;
using AuthMicroservice.BLL.Services.Interfaces;
using AuthMicroservice.DAL.Repositories.Interfaces;
using AutoMapper;
using Microservice.Messages.Infrastructure.OperationResult;
using Microservice.Messages.Infrastructure.UnitofWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthMicroservice.BLL.Services.Classes
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public OperationResult<List<UserDTO>> GetUsers()
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
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
