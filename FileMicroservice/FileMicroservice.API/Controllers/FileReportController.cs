using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileMicroservice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microservice.Core.Infrastructure.Filters;
using Microservice.Core.Infrastructure.OperationResult;
using FileMicroservice.API.Models;
using AutoMapper;
using FileMicroservice.BLL.Models.File;
using FileMicroservice.BLL.Models.DTO;

namespace FileMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/file/report")]
    public class FileReportController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public FileReportController(IFileService fileService, IMapper mapper)
        {
            _fileService = fileService;
            _mapper = mapper;
        }

        [HttpPost]
        [Produces(typeof(OperationResult<FileDTO>))]
        [ControllerActionFilter]
        public async Task<ActionResult> UploadFile([FromForm] FileUploadAPI newFile)
        {          
            var result = await _fileService.Upload(_mapper.Map<FilePost>(newFile));

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> DownloadFile(Guid id)
        {
            var result = await _fileService.Download(new FileGet 
            { 
                Id = id
            });

            if (result.IsSuccess)
            {
                return File(result.Data.FileStream, result.Data.Mime, result.Data.Name);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
