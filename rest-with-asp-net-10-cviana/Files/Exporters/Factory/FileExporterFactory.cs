using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Files.Exporters.Contract;
using rest_with_asp_net_10_cviana.Files.Exporters.Impl;
using rest_with_asp_net_10_cviana.Files.Importers.Contract;
using rest_with_asp_net_10_cviana.Files.Importers.Factory;
using rest_with_asp_net_10_cviana.Files.Importers.Impl;

namespace rest_with_asp_net_10_cviana.Files.Exporters.Factory
{
    public class FileExporterFactory(IServiceProvider serviceProvider, ILogger<FileExporterFactory> logger)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<FileExporterFactory> _logger = logger;

        public IFileExporter GetExporter(string acceptHeaderValue)
        {
            string fileType = acceptHeaderValue.ToLower();
            _logger.LogInformation("Client required {fileType} file exporter", fileType);

            switch (fileType)
            {
                case FileMediaTypes.CSV:
                    return _serviceProvider.GetRequiredService<CsvFileExporter>();
                case FileMediaTypes.XLSX:
                    return _serviceProvider.GetRequiredService<XlsxFileExporter>();
                default:
                    _logger.LogError("Unsupported file format: {fileType}", fileType);
                    throw new NotSupportedException($"File format {fileType} not supported");
            }
        }
    }
}
