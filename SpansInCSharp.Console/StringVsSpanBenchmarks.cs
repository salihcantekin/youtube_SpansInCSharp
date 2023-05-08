using BenchmarkDotNet.Attributes;

namespace SpansInCSharp.Console;

[MemoryDiagnoser]
public class StringVsSpanBenchmarks
{
    private string input;
    private readonly string searchTerm = "example";
    private readonly string replaceTerm = "sample";
    private readonly string startsWith = "This";

    private ReadOnlySpan<char> InputSpan => input.AsSpan();
    private ReadOnlySpan<char> SearchTermSpan => searchTerm.AsSpan();
    private ReadOnlySpan<char> ReplaceTermSpan => replaceTerm.AsSpan();
    private ReadOnlySpan<char> StartsWithSpan => startsWith.AsSpan();

    [Params(1000)]
    public int Iterations { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        input = string.Concat(Enumerable.Repeat("This is an example sentence. ", Iterations));
    }

    [Benchmark]
    public string StringReplace() => input.Replace(searchTerm, replaceTerm);

    [Benchmark]
    public string SpanReplace()
    {
        return ReplaceSpan(InputSpan, SearchTermSpan, ReplaceTermSpan).ToString();
    }

    [Benchmark]
    public string StringSubstring() => input[..20];

    [Benchmark]
    public ReadOnlySpan<char> SpanSlice() => InputSpan[..20];

    [Benchmark]
    public bool StringStartsWith() => input.StartsWith(startsWith);

    [Benchmark]
    public bool SpanStartsWith() => InputSpan.StartsWith(StartsWithSpan);

    private static ReadOnlySpan<char> ReplaceSpan(ReadOnlySpan<char> source, ReadOnlySpan<char> oldValue, ReadOnlySpan<char> newValue)
    {
        int index = source.IndexOf(oldValue);
        if (index == -1) return source;

        char[] result = new char[source.Length + newValue.Length - oldValue.Length];
        Span<char> resultSpan = result;

        source.Slice(0, index).CopyTo(resultSpan);
        newValue.CopyTo(resultSpan.Slice(index));
        source.Slice(index + oldValue.Length).CopyTo(resultSpan.Slice(index + newValue.Length));

        return resultSpan;
    }
}
