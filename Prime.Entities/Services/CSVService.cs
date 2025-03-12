using CsvHelper;
using System.Globalization;

namespace  CSV
{
    public class CSVService : ICSVService
    {
        public List<T> ReadCSV<T>(Stream file)
        {
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<T>().ToList();
            return records;
        }
    }
}
