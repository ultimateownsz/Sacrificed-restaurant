namespace Project;
internal class SelectionLogic
{
    static protected Tuple<T1?, int> ReverseLookup<T1, T2>(Dictionary<T1, T2> dict, T2 target) where T1 : notnull
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

        return new(default(T1), 0);
    }

    static protected Index Next(int size, int current, bool reverse = false)
    {
        current += (reverse) ? -1 : 1;

        if (current >= size) current = 0;
        if (current < 0) current = size - 1;

        return current;
    }

    static protected Dictionary<string, bool> ToSelectable(List<string> options)
    {
        // transform list to dictionary with booleans
        // where the booleans are what's selected
        Dictionary<string, bool> selection = new();
        foreach (var option in options)
        {
            selection.Add(option, false);
        }
        selection[options[0]] = true;

        return selection;
    }
}
