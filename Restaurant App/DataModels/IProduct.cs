public interface IProduct
{
    string ProductName { get; set; }
    Dictionary<string, object> CustomAttributes { get; set; }
}