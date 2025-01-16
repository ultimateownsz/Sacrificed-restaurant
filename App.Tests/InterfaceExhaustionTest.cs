using Restaurant;
namespace App.Tests;

[TestClass] // (only works on VS??)
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

    public void MultiExhaust(string[] options)
    {

        List<ConsoleKey> keystrokes = new();
        for (int i = 0; i < (options.Length) + 1; i++)
        {
            // select options
            for (int c = 0; c < i; c++)
            {
                keystrokes.Add(ConsoleKey.Enter);
                keystrokes.Add(ConsoleKey.DownArrow);
            }

            // proceed iteration
            for (int r = 0; r < (options.Length - i); r++)
            {
                keystrokes.Add(ConsoleKey.DownArrow);
            }
            keystrokes.Add(ConsoleKey.Enter);

            // execute with simulated keystrokes
            List<SelectionLogic.Selection> collection = SelectionPresent.Show(
                options.ToList(), keystrokes: keystrokes, mode: SelectionLogic.Mode.Multi);

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

    public void ErroneousExhaust(string[] options, SelectionLogic.Mode mode)
    {
     
        // register every key
        List<ConsoleKey> keystrokes = new();
        foreach (var item in Enum.GetValues(typeof(ConsoleKey)).Cast<ConsoleKey>())
            keystrokes.Add(item);

        // exclude intended
        List<ConsoleKey> intended = new() { ConsoleKey.UpArrow, ConsoleKey.DownArrow,
                                            ConsoleKey.Enter, ConsoleKey.Escape };
        foreach (var key in intended)
            keystrokes.Remove(key);

        // install termination
        if (mode == SelectionLogic.Mode.Multi)
        {
            // equalize expected output
            keystrokes.Add(ConsoleKey.Enter);
            keystrokes.Add(ConsoleKey.UpArrow);
        }
        keystrokes.Add(ConsoleKey.Enter);

        // assertion
        Assert.AreEqual(SelectionPresent.Show(options.ToList(), keystrokes: 
            keystrokes, mode: mode).ElementAt(0).text, options.First());
        
    }

    [TestMethod]
    [DataRow(["option 1", "option 2", "option 3"])]
    public void OmniExhaust(string[] options)
    {
        foreach (var mode in new List<SelectionLogic.Mode> () { 
            SelectionLogic.Mode.Single, SelectionLogic.Mode.Multi, SelectionLogic.Mode.Scroll })
        {

            // exhaustive I/O
            if (mode != SelectionLogic.Mode.Multi)
                UniExhaust(options, mode);
            else
                MultiExhaust(options);

            // inject faulty keystrokes
            ErroneousExhaust(options, mode);
            
            // terminate upon initialization
            Assert.AreEqual(SelectionPresent.Show(options.ToList(), 
                keystrokes: [ConsoleKey.Escape], mode: mode).ElementAt(0).index, -1);
        }

    }

}
