using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Cells;
using Microsoft.AspNetCore.Hosting;
using KJ1012.Core.Data;
using KJ1012.Data.Entities;
using KJ1012.Domain;

namespace KJ1012.Services
{
    public abstract class BaseExportService<T>:BaseService<T>,IBaseExportService<T> where T : BaseEntity
    {
        private readonly IWebHostEnvironment _environment;

        public BaseExportService(IWebHostEnvironment environment, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _environment = environment;
        }
        public abstract Task<Stream> ExportToXlsxStream(SearchData searchData);

        public virtual async Task<string> ExportToPdf(SearchData searchData, string pdfName = "")
        {
            var stream = await ExportToXlsxStream(searchData);
            Workbook wb = new Workbook(stream);
            string path = string.Concat(_environment.WebRootPath, "\\", ConstDefine.PdfFilePath);
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (string.IsNullOrEmpty(pdfName))
                pdfName = Guid.NewGuid() + ".pdf";
            else
                pdfName = pdfName + ".pdf";
            string pdfFileName =
                string.Concat(path, pdfName);
            wb.Save(pdfFileName, SaveFormat.Pdf);
            return pdfName;
        }

    }
}
