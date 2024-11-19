namespace Project.Logic;
internal class SelectionMenuLogic
{
    static protected Tuple<T?, int> ReverseLookup<T, T2>(Dictionary<T, T2> dict, T2 target) where T : notnull
    {
        int i = 0;
        foreach ((T key, T2 value) in dict)
        {
            if (value == null)
                continue;

            if (value.Equals(target))
                return new(key, i);

            i++;
        }

        // make the values exist,
        // otherwise issues may arise
        return new(default(T), 0);
    }

    static protected Index Next(int size, int current, bool reverse = false)
    {
        current += (reverse) ? -1 : 1;

        if (current >= size) current = 0;
        if (current < 0) current = size - 1;

        return current;
    }

    static protected Dictionary<string, bool> ConstructSelection(List<string> options)
    {
        Dictionary<string, bool> selection = new();
        foreach (var option in options)
        {
            selection.Add(option, false);
        }
        selection[options[0]] = true;

        return selection;
    }
}
