using System.IO;
using System.Threading.Tasks;
using KJ1012.Core.Data;
using KJ1012.Data.Entities;

namespace KJ1012.Services
{
    public interface IBaseExportService<T> : IBaseService<T> where T : BaseEntity
    {
       Task<Stream> ExportToXlsxStream(SearchData searchData);
       Task<string> ExportToPdf(SearchData searchData, string pdfName = "");
   }
}
