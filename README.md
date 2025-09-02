# P3k.FeaturesDependencyInjector

A lightweight dependency injection container with automatic feature installer discovery. Built on top of P3k.FeaturesDependencyInstaller for modular service registration.


## Installation

**Option 1:** Add via git url in Unity Package Manager or add manually to your Unity project's `manifest.json`:
```json
{
  "dependencies": {
    "com.p3k.featuresdependencyinjector": "https://github.com/p3k22/FeaturesDependencyInjector.git"
  }
}
```

**Option 2:** Copy the runtime folder into your project. If you're using assembly definitions, add a reference to the package asmdef. If not using asmdefs, delete the included asmdef file.


## Dependencies
This feature depends on another package: https://github.com/p3k22/FeaturesDependencyInstaller.git

If installed via option 2, you will need to download the dependency package separately.

The dependency package will not be auto discovered via Unity's Package Manager, you will still have to download the dependency package separately.
Alternatively, download P3k's Auto Sync Git Package Manager: git@github.com:p3k22/AutoSyncGitPackageManager.git which does support the "gitdependency" tag within this package and will auto discover package dependencies.



## Quick Start

```csharp
// Bootstrap your application
var serviceProvider = Bootstrapper.Initialize();

// Use services
var myService = serviceProvider.GetService<IMyService>();
```

## Advanced Usage

```csharp
// Filter installers by namespace
var serviceProvider = Bootstrapper.Initialize("MyApp.Features");

// Add extra services
var serviceProvider = Bootstrapper.Initialize(configure: services =>
{
    services.AddSingleton<IExtraService, ExtraService>();
});
```

## Manual Registration

```csharp
var services = new ServiceCollection();
services.AddSingleton<IMyService, MyService>();
services.AddTransient<IRepository, Repository>();

var provider = services.BuildServiceProvider();
```

## Features

- **Automatic Discovery**: Finds all `IFeatureInstaller` implementations
- **Namespace Filtering**: Load only specific feature installers  
- **Simple API**: Clean service registration methods
- **Lifecycle Management**: Singleton and transient lifetimes
- **IEnumerable Support**: Resolves multiple implementations
- **Factory Support**: Custom instance creation
- **Disposable**: Proper cleanup of singleton instances