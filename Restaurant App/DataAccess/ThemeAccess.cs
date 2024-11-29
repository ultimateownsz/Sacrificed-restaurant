namespace Project;
public class ThemeAccess : DataAccess<ThemeModel>
{
    public ThemeAccess() : base(typeof(ThemeModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
