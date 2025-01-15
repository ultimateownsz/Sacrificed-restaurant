using App.DataModels.Utils;

namespace App.DataModels.Product;
public class PairModel : IModel
{
    public int? ID { get; set; }
    public int? FoodID { get; set; }
    public int? DrinkID { get; set; }

    public PairModel(int? foodID, int? drinkID, int? id = null)
    {
        ID = id;
        FoodID = foodID;
        DrinkID = drinkID;
    }
}
