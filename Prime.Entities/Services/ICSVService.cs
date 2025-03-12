namespace CSV
{
    public interface ICSVService
    {
        public List<T> ReadCSV<T>(Stream file);
    }
}
