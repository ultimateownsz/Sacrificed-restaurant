public interface IProduct
{
    string ProductName { get; set; }

    // can be used to store custom attributes, like craft beers or normal products
    // Dictionary<string, object> CustomAttributes { get; set; }
}