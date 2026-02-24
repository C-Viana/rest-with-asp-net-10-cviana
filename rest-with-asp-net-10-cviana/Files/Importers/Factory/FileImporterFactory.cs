using rest_with_asp_net_10_cviana.Files.Importers.Contract;
using rest_with_asp_net_10_cviana.Files.Importers.Impl;

namespace rest_with_asp_net_10_cviana.Files.Importers.Factory
{
    public class FileImporterFactory(IServiceProvider serviceProvider, ILogger<FileImporterFactory> logger)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<FileImporterFactory> _logger = logger;

        public IFileImporter GetImporter(string fileName)
        {
            string fileExtension = fileName.Substring(fileName.LastIndexOf('.'));
            _logger.LogInformation("Selected {fileExtension} file importer for file: {fileName}", fileExtension, fileName);

            switch (fileExtension)
            {
                case ".csv":
                    return _serviceProvider.GetRequiredService<CsvFileImporter>();
                case ".xlsx":
                    return _serviceProvider.GetRequiredService<XlsxFileImporter>();
                default:
                    _logger.LogError("Unsupported file format: {fileExtension}", fileExtension);
                    throw new NotSupportedException($"File format {fileExtension} not supported");
            }
        }

    }
}
