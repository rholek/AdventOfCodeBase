using System.Collections.Concurrent;
using System.Text;

namespace AdventOfCode.TextStreams;

public class BlockingTextWriter : TextWriter
{
    private readonly BlockingCollection<string> sharedCollection;

    public BlockingTextWriter(BlockingCollection<string> sharedCollection)
    {
        this.sharedCollection = sharedCollection;
    }

    public override Encoding Encoding => Encoding.UTF8;

    public override void WriteLine(int value)
    {
        sharedCollection.Add(value.ToString());
    }
}