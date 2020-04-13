namespace Assignment.Model.Abstractions
{
    public interface IConfigProvider
    {
        string ConnectionString { get; set; }
    }
}
