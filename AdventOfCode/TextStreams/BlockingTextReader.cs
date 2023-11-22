using System.Collections.Concurrent;

namespace AdventOfCode.TextStreams;

public class BlockingTextReader : TextReader
{
    private readonly IEnumerator<string> enumerator;

    public BlockingTextReader(BlockingCollection<string> blockingCollection)
    {
        enumerator = blockingCollection.GetConsumingEnumerable().GetEnumerator();
    }

    public override string ReadLine()
    {
        enumerator.MoveNext();
        return enumerator.Current;
    }
}