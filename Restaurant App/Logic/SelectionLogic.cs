namespace Project;
internal class SelectionLogic
{
    public enum Mode
    {
        Single,
        Multi,
        Narrow,
    }

    public static Tuple<T1?, int> ReverseLookup<T1, T2>(Dictionary<T1, T2> dict, T2 target) where T1 : notnull
    {
        int i = 0;
        foreach ((T1 key, T2 value) in dict)
        {
            if (value == null)
                continue;

            if (value.Equals(target))
                return new(key, i);

            i++;
        }

        return new(default, 0);
    }

    public static Index Next(int size, int current, bool reverse = false)
    {
        current += (reverse) ? -1 : 1;

        if (current >= size) current = 0;
        if (current < 0) current = size - 1;

        return current;
    }

    public static Dictionary<string, bool> ToSelectable(List<string?> options, SelectionLogic.Mode mode)
    {
        // transform list to dictionary with booleans
        // where the booleans are what's selected
        Dictionary<string, bool> selection = new();
        foreach (var option in options)
        {
            if (option == null) continue;
            selection.Add(option, false);
        }
        selection[options[(mode ==  SelectionLogic.Mode.Narrow) ? options.Count - 1 : 0] ?? "<NULL> OPTION"] = true;

        // new termination
        if (mode == SelectionLogic.Mode.Multi)
        {
            selection.Add("continue", false);
        }

        return selection;
    }
}
