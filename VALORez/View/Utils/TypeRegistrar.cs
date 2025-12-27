using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
namespace View.Utils;

public class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _builder;
    public TypeRegistrar(IServiceCollection builder) => _builder = builder;
    public ITypeResolver Build() => new TypeResolver(_builder.BuildServiceProvider());
    public void Register(Type service, Type implementation) => _builder.AddSingleton(service, implementation);
    public void RegisterInstance(Type service, object implementation) => _builder.AddSingleton(service, implementation);
    public void RegisterLazy(Type service, Func<object> factory) => _builder.AddSingleton(service, _ => factory());
}

public class TypeResolver : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _provider;
    public TypeResolver(IServiceProvider provider) => _provider = provider;
    public object Resolve(Type? type) => _provider.GetService(type);
    public void Dispose() { if (_provider is IDisposable d) d.Dispose(); }
}