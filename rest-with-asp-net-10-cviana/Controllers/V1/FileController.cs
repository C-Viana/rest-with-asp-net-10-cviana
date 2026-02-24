using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Services;

namespace rest_with_asp_net_10_cviana.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    [Authorize("Bearer")]
    public class FileController : ControllerBase
    {
        private readonly IFileServices _service;
        private readonly ILogger<FileController> _logger;

        public FileController(IFileServices service, ILogger<FileController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("file")]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/octet-string")]
        public IActionResult DownloadFile([FromForm] string filename)
        {
            _logger.LogInformation("Fetching file {fileName} on server", filename);
            byte[] buffer = _service.GetFile(filename);
            var contentType = $"application/{Path.GetExtension(filename).TrimStart('.')}";
            
            if(buffer == null || buffer.Length == 0) return NoContent();

            _logger.LogInformation("Sending requested file to client");
            return File(buffer, contentType, filename);
        }

        [HttpPost("upload")]
        [ProducesResponseType(201, Type = typeof(FileDetailDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> SaveFileToDisk([FromForm] FileUploadDTO file)
        {
            _logger.LogInformation("Saving file {fileName} on server", file.File.FileName);
            FileDetailDTO fileDetailDTO = await _service.SaveFileToDisk(file.File);

            return Created(fileDetailDTO.DocumentUrl, fileDetailDTO);
        }

        [HttpPost("upload/files")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200, Type = typeof(List<FileUploadDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> SaveFileToDisk([FromForm] MultipleFilesUploadDTO files)
        {
            _logger.LogInformation("Saving files {files} on server", files.Files.Select(item => item.FileName).ToList() );
            List<FileDetailDTO> fileDetailDTO = await _service.SaveFilesToDisk(files.Files);

            return Ok(fileDetailDTO);
        }
    }
}
