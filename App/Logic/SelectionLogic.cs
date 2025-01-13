namespace Restaurant;
internal class SelectionLogic
{
    public enum Mode
    {
        Multi,
        Scroll,
        Single,
    }

    public enum Interaction
    {
        None,
        Moved,
        Marked,
        Selected,
        Terminated,
    }

    public struct Selection()
    {
        public string? text;
        public int index;
    }

    public struct Selectable()
    {
        public int index = 0;
        public bool selected = false;
        public bool highlighted = false;
    }

    public static Dictionary<string, Selectable> ToSelectables(List<string> options,
        List<string>? preselected, Mode mode)
    {
        // make the scroll functionality more fluent
        if (mode == Mode.Scroll) options.Reverse();
        
        // construct dictionary + initiate index
        var dict = new Dictionary<string, Selectable>(); int c = 0;
        options.ForEach(x => dict.Add(x, new() { index = c++ }));

        // highlight preselected
        foreach (var pair in dict)
        {
            if ((preselected ?? []).Contains(pair.Key))
            {
                dict[pair.Key] = new()
                {
                    index = dict[pair.Key].index,
                    selected = dict[pair.Key].selected,
                    highlighted = true
                };
            }
        }

        // insert default submit in multi-select mode
        if (mode == Mode.Multi)
            dict["continue"] = new()
            {
                index = c++,
                selected = false,
                highlighted = false
            };

        // set default & return
        int index = (mode == Mode.Scroll) ? c - 1 : 0;
        dict[options.ElementAt(index)] = new()
        {
            index = index,
            selected = true,
            highlighted = dict[options.ElementAt(index)].highlighted
        };
        return dict;
    }

    public static void Iterate(Dictionary<string, Selectable> selection, bool reverse = false)
    {
        // find currently selected
        KeyValuePair<string, Selectable> selected =
            selection.Where(x => x.Value.selected == true).ElementAt(0);

        // find next
        Selectable values = selected.Value;
        int next = (reverse) ? values.index - 1 : values.index + 1;

        // handle overflows
        if ((reverse && next < 0) || (next >= selection.Count()))
            next = (reverse) ? selection.Count() - 1 : 0;

        // reselect
        foreach (string key in new List<string>() {
            selected.Key, selection.Keys.ElementAt(new Index(next)) })
        {
            selection[key] = new()
            {
                index = selection[key].index,
                highlighted = selection[key].highlighted,
                selected = !selection[key].selected,
            };
        }
    }

    public static void Mark(Dictionary<string, Selectable> selection)
    {
        // target current
        var current = selection
            .Where(x => x.Value.selected == true)
            .Select(x => x.Key).ElementAt(0);

        // modify
        selection[current] = new()
        {
            index = selection[current].index,
            highlighted = !selection[current].highlighted,
            selected = selection[current].selected,
        };
    }
}
