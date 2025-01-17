namespace Restaurant;
internal class PairPresent
{
    public static void Show(ref PairLogic.Input input, ref PairLogic.Output output) 
        
        =>  input.Product = SelectionPresent.Show(
            
            options       : output.Products, 
            preselected   : output.Highlights, 
            banner        : "PAIR MENU", 
            mode          : SelectionLogic.Mode.Single
        
        ).ElementAt(0).text;
}
