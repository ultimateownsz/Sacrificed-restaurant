// enums are used to define the types of data that can be stored in the database
// usefull against refactoring or typechecking
// in the ProductCategory.cs file I made a switch case for each enum to return the correct string
// 

public class ProductCategoryType
{
    public const string MainDishes = "Main Dishes";
    public const string SideDishes = "Side Dishes";
    public const string NonAlcoholicBeverages = "Non Alcoholic Beverages";
    public const string AlcoholicBeverages = "Alcoholic Beverages";
    public const string Desserts = "Desserts";
}
