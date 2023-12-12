namespace AdventOfCodeBase.Tests;

public class MapTest
{
    [Theory]
    [InlineData(3, 1, 
                   """
                   0123456789
                   ABCDEFGHIJ
                   """,
                   """
                   012 3456789
                   ABC DEFGHIJ
                   """)]
    [InlineData(3, 5, 
                   """
                   0123456789
                   ABCDEFGHIJ
                   """,
                   """
                   012     3456789
                   ABC     DEFGHIJ
                   """)]
    [InlineData(3, 0,
                   """
                   0123456789
                   ABCDEFGHIJ
                   """,
                   """
                   0123456789
                   ABCDEFGHIJ
                   """)]
    [InlineData(3, -2,
                   """
                   0123456789
                   ABCDEFGHIJ
                   """,
                   """
                   0123  456789
                   ABCD  EFGHIJ
                   """)]
    public void Map_ShiftColumnTest(int column, int step, string mapString, string expectedMapString)
    {
        var map = new Map<string>(mapString.SplitByLine());
        map.ShiftColumn(column, step);
        map.PrintToString(" ").TrimEnd().Should().Be(expectedMapString);
    }

    [Theory]
    [InlineData(2, 1,
        """
        0AZ
        1BY
        2CX
        3DW
        4EV
        """,
        """
        0AZ
        1BY
        
        2CX
        3DW
        4EV
        """)]
    [InlineData(2, 0,
        """
        0AZ
        1BY
        2CX
        3DW
        4EV
        """,
        """
        0AZ
        1BY
        2CX
        3DW
        4EV
        """)]
    [InlineData(2, -4,
        """
        0AZ
        1BY
        2CX
        3DW
        4EV
        """,
        """
        0AZ
        1BY
        2CX
        
        
        
        
        3DW
        4EV
        """)]
    public void Map_ShiftRowTest(int row, int step, string mapString, string expectedMapString)
    {
        var map = new Map<string>(mapString.SplitByLine());
        map.ShiftRow(row, step);
        map.PrintToString("").TrimEnd().Should().Be(expectedMapString);
    }
}