using Restaurant;
using System.Diagnostics;

namespace App.Tests;

[TestClass]
public class SelectionTest
{
    private void _exhaustive_direct(string[] options, SelectionLogic.Mode mode)
    {
        List<ConsoleKey> keystrokes = new() { ConsoleKey.Enter };
        for (int i = 0;  i < options.Length; i++)
        {

            // execute with simulated keystrokes
            SelectionLogic.Selection selection = SelectionPresent.Show(options.ToList(), 
                keystrokes: keystrokes, mode: mode).ElementAt(0);

            // assertion (this assert fails because the actual code doesn't work)
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

    private void _exhaustive_multi(string[] options) { }

    [TestMethod]
    [DataRow(["option 1", "option 2", "option 3"])]

    public void ExhaustiveIO(string[] options)
    {
        _exhaustive_direct(options, SelectionLogic.Mode.Single);
        _exhaustive_direct(options, SelectionLogic.Mode.Scroll);
        
        _exhaustive_multi(options);
    }

}
