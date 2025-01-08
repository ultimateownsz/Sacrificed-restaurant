namespace Project;
public class PairModel : IModel
{
    public int? ID { get; set; }
    public int? FoodID { get; set; }
    public int? DrinkID { get; set; }

    public PairModel() { }
    public PairModel(int? foodID, int? drinkID, int? id = null)
    {
        ID = id;
        FoodID = foodID;
        DrinkID = drinkID;
    }
}
