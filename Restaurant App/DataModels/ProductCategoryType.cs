// these constants define the allowed values for the product category type
// they are used to ensure data consistency and prevent invalid data being stored in the db
// in the ProductCategory.cs file, a switch case is used to map each constant to a string value

public class ProductCategoryType
{
    public const string MainDishes = "Main Dishes";
    public const string SideDishes = "Side Dishes";
    public const string NonAlcoholicBeverages = "Non Alcoholic Beverages";
    public const string AlcoholicBeverages = "Alcoholic Beverages";
    public const string Desserts = "Desserts";
}
