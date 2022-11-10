namespace dnWalker.Configuration;

public interface IConfigurationBuilder : IConfiguration
{
    void SetValue(string key, object? value);
    void AddProvider(IConfigurationProvider provider);
    
    IConfiguration Build();
}