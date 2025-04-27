using System.Diagnostics;
using System.Reflection;

namespace CollectionViewIssue.Model;

[DebuggerDisplay("{Name, nq}")]
internal class NamedColor : IEquatable<NamedColor>
{
    class NamedColorComparer : IComparer<NamedColor>
    {
        public int Compare(NamedColor x, NamedColor y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return StringComparer.InvariantCulture.Compare(x.Name, y.Name);
        }
    }

    public static IComparer<NamedColor> Comparer = new NamedColorComparer();
    static readonly List<NamedColor> _colors;

    static NamedColor()
    {
        List<NamedColor> colors = new();
        foreach (FieldInfo info in typeof(Microsoft.Maui.Graphics.Colors).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (info.FieldType == typeof(Color))
            {
                object infoValue = info.GetValue(null);
                if (infoValue == null)
                {
                    continue;
                }
                NamedColor color = new NamedColor(info.Name, (Color)infoValue);
                colors.Add(color);
            }
        }
        colors.Sort(Comparer);
        _colors = colors;
    }

    public NamedColor(string name, Color color)
    {
        Name = name;
        Color = color;
    }

    public Color Color { get;}
    public string Name { get; }


    public override bool Equals(object obj)
    {
        return Comparer.Compare(this, obj as NamedColor) == 0;
    }

    public bool Equals(NamedColor other)
    {
        return Comparer.Compare(this, other) == 0;
    }

    public override int GetHashCode()
    {
        return Color.GetHashCode();
    }

    public static IReadOnlyList<NamedColor> All
    {
        get => _colors;
    }
}

