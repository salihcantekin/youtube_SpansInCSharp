


using BenchmarkDotNet.Running;
using SpansInCSharp.Console;

BenchmarkRunner.Run<StringVsSpanBenchmarks>();
return;




// String vs Span<T>

string input = "This is an example sentence.";

ReadOnlySpan<char> inputSpan = input.AsSpan();

Span<char> a = new[] { 'A', 'B' };

a = "salih".ToArray();
a = "salh".ToCharArray();
Span<char> c = a.Slice(0, 2);

string name = "salih";

var newName = name.Substring(0, 2);

var firstTwo = "sa";

