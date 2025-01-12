public class TableSelectionLogic
{
    public string[] GenerateClearedRows(int gridHeight, int gridWidth)
    {
        string[] clearedRows = new string[gridHeight];
        for (int i = 0; i < gridHeight; i++)
        {
            clearedRows[i] = new string(' ', gridWidth);
        }

        return clearedRows;
    }
}
