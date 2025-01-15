using Restaurant;
namespace App.Tests;

//[TestClass] (only works on VS)
public class InterfaceExhaustionTest
{
    public void UniExhaust(string[] options, SelectionLogic.Mode mode)
    {
        List<ConsoleKey> keystrokes = new() { ConsoleKey.Enter };
        for (int i = 0;  i < options.Length; i++)
        {

            // execute with simulated keystrokes
            SelectionLogic.Selection selection = SelectionPresent.Show(options.ToList(), 
                keystrokes: keystrokes, mode: mode).ElementAt(0);

            // assertion
            Assert.AreEqual(i, selection.index, message: "indexes do not match");
            Assert.AreEqual(options[i], selection.text, message: "texts do not match");
            
            // move an option down
            for (int j = 0; j <= i; j++)
            {
                var key = (mode == SelectionLogic.Mode.Scroll)
                    ? ConsoleKey.UpArrow : ConsoleKey.DownArrow;
                
                keystrokes.Add(key);
            }
            
            // interact with option
            keystrokes.Add(ConsoleKey.Enter);
        }
    }

    public void MultiExhaust(List<string> options)
    {

        List<ConsoleKey> keystrokes = new();
        for (int i = 0; i < (options.Count) + 1; i++)
        {
            // select options
            for (int c = 0; c < i; c++)
            {
                keystrokes.Add(ConsoleKey.Enter);
                keystrokes.Add(ConsoleKey.DownArrow);
            }

            // proceed iteration
            for (int r = 0; r < (options.Count - i); r++)
            {
                keystrokes.Add(ConsoleKey.DownArrow);
            }
            keystrokes.Add(ConsoleKey.Enter);

            // execute with simulated keystrokes
            List<SelectionLogic.Selection> collection = SelectionPresent.Show(
                options, keystrokes: keystrokes, mode: SelectionLogic.Mode.Multi);

            for (int o = 0; o < collection.Count; o++)
            {
                SelectionLogic.Selection selected = collection.ElementAt(o);
                if (selected.text == "") continue;

                // assertion
                Assert.AreEqual<int>(o, selected.index, "indexes do not match");
                Assert.AreEqual<string?>(options[o], selected.text, "texts do not match");
            }

        }
    }

    [TestMethod]
    [DataRow(["option 1", "option 2", "option 3"])]
    public void OmniExhaust(string[] options)
    {
        // uni-interact methods (click & proceed)
        UniExhaust(options, SelectionLogic.Mode.Single);
        UniExhaust(options, SelectionLogic.Mode.Scroll);

        // multi-interact methods (select [multiple] & proceed)
        MultiExhaust(options.ToList());
    }

}
