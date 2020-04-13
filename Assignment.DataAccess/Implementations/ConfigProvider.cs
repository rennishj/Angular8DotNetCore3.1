using Assignment.Model.Abstractions;

namespace Assignment.Model.Implementations
{
    public class ConfigProvider : IConfigProvider
    {
        public string ConnectionString { get; set; }
    }
}
