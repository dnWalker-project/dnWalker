dotnet build Examples\Examples.csproj --configuration Release
dotnet build dnWalker.Benchmarks\dnWalker.Benchmarks.csproj --configuration Release

./dnWalker.Benchmarks/bin/Release/net7.0/dnWalker.Benchmarks.exe `
  --assembly Examples/bin/Release/net7.0/Examples.dll `
  --test-output Examples.Tests.Generated `
  --expl-output benchmark-explorations `
  --stats benchmark-explorations/stats.csv `
  --methods benchmark_methods.txt `
  --repetitions 10 `
  --warm-up 5
