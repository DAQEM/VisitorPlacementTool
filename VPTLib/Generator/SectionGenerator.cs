namespace VPTLib;

public static class SectionGenerator
{
    public static List<Section> GenerateSections(int[,] rowsAndColumns)
    {
        List<Section> sections = new();
        for (int i = 0; i < rowsAndColumns.GetLength(0); i++)
        {
            sections.Add(GenerateSection(rowsAndColumns[i, 0], rowsAndColumns[i, 1]));
        }
        return sections;
    }

    public static Section GenerateSection(int rows, int columns)
    {
        return new Section(rows, columns);
    }
}