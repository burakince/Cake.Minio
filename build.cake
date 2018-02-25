var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = "Cake.Minio.sln";
var appName = "Cake.Minio";

var apiKey = EnvironmentVariable("NUGET_API_KEY") ?? "abcdef0123456789";
var buildNumber = EnvironmentVariable("APPVEYOR_BUILD_NUMBER") ?? "0";

var version = EnvironmentVariable("APPVEYOR_REPO_TAG_NAME") ?? "1.0.0";

Setup(context =>
{
    if (!DirectoryExists("nuget"))
    {
        CreateDirectory("nuget");
    }
});

Task("Clean")
    .Does(() =>
    {
        CleanDirectory("nuget");
    });

Task("Restore")
    .Does(() =>
    {
        NuGetRestore(solution);
    });

Task("Build")
    .Does(() =>
    {
        MSBuild(solution, new MSBuildSettings
        {
            Verbosity = Verbosity.Minimal,
            Configuration = configuration
        });
    });

Task("Test")
    .Does(() =>
    {
        var testProjects = GetFiles("./test/**/*.csproj");
        foreach (var testProject in testProjects)
        {
            var projectFile = MakeAbsolute(testProject).ToString();
            var dotNetTestSettings = new DotNetCoreTestSettings
            {
                Configuration = configuration,
                NoBuild = true
            };

            DotNetCoreTest(projectFile, dotNetTestSettings);
        }
    });

Task("Pack")
    .Does(() =>
    {
        var nuGetPackSettings = new NuGetPackSettings
        {
            Id = appName,
            Version = version,
            Title = appName,
            Authors = new[] { "Burak İnce" },
            Owners = new[] { "Burak İnce", "cake-contrib", "minio" },
            Description = "Minio Cake Plugin for Amazon S3 Compatible Cloud Storage.",
            Summary = "Minio Cake Plugin for Amazon S3 Compatible Cloud Storage.",
            IconUrl = new Uri("https://cdn.rawgit.com/cake-contrib/graphics/a5cf0f881c390650144b2243ae551d5b9f836196/png/cake-contrib-medium.png"),
            ProjectUrl = new Uri("https://github.com/burakince/Cake.Minio"),
            LicenseUrl = new Uri("https://github.com/burakince/Cake.Minio/blob/master/LICENSE"),
            Tags = new [] { "Cake", "Minio", "Cloud", "Storage" },
            RequireLicenseAcceptance = false,
            Symbols = false,
            NoPackageAnalysis = true,
            Files = new [] 
            {
                new NuSpecContent
                {
                    Source = "netstandard1.6/Cake.Minio.dll",
                    Target = "lib/netstandard1.6"
                },
                new NuSpecContent
                {
                    Source = "netstandard1.6/Cake.Minio.xml",
                    Target = "lib/netstandard1.6"
                },
                new NuSpecContent
                {
                    Source = "net452/Cake.Minio.dll",
                    Target = "lib/net452"
                },
                new NuSpecContent
                {
                    Source = "net452/Cake.Minio.xml",
                    Target = "lib/net452"
                },
                new NuSpecContent
                {
                    Source = "net452/Minio.dll",
                    Target = "lib/net452"
                },
                new NuSpecContent
                {
                    Source = "net452/Newtonsoft.Json.dll",
                    Target = "lib/net452"
                },
                new NuSpecContent
                {
                    Source = "net452/RestSharp.dll",
                    Target = "lib/net452"
                },
                new NuSpecContent
                {
                    Source = "net452/System.Reactive.Core.dll",
                    Target = "lib/net452"
                },
                new NuSpecContent
                {
                    Source = "net452/System.Reactive.Interfaces.dll",
                    Target = "lib/net452"
                },
                new NuSpecContent
                {
                    Source = "net452/System.Reactive.Linq.dll",
                    Target = "lib/net452"
                },
                new NuSpecContent
                {
                    Source = "net452/System.Reactive.PlatformServices.dll",
                    Target = "lib/net452"
                },
                new NuSpecContent
                {
                    Source = "net452/System.Reactive.Windows.Threading.dll",
                    Target = "lib/net452"
                },
                new NuSpecContent
                {
                    Source = "net46/Cake.Minio.dll",
                    Target = "lib/net46"
                },
                new NuSpecContent
                {
                    Source = "net46/Cake.Minio.xml",
                    Target = "lib/net46"
                },
                new NuSpecContent
                {
                    Source = "net46/Minio.dll",
                    Target = "lib/net46"
                },
                new NuSpecContent
                {
                    Source = "net46/Newtonsoft.Json.dll",
                    Target = "lib/net46"
                },
                new NuSpecContent
                {
                    Source = "net46/RestSharp.dll",
                    Target = "lib/net46"
                },
                new NuSpecContent
                {
                    Source = "net46/System.Reactive.Core.dll",
                    Target = "lib/net46"
                },
                new NuSpecContent
                {
                    Source = "net46/System.Reactive.Interfaces.dll",
                    Target = "lib/net46"
                },
                new NuSpecContent
                {
                    Source = "net46/System.Reactive.Linq.dll",
                    Target = "lib/net46"
                },
                new NuSpecContent
                {
                    Source = "net46/System.Reactive.PlatformServices.dll",
                    Target = "lib/net46"
                },
                new NuSpecContent
                {
                    Source = "net46/System.Reactive.Windows.Threading.dll",
                    Target = "lib/net46"
                }
            },
            Dependencies = new [] {
                new NuSpecDependency
                {
                    Id = "Cake.Core",
                    TargetFramework = "netstandard1.6",
                    Version = "0.22.0"
                },
                new NuSpecDependency
                {
                    Id = "Cake.Core",
                    TargetFramework = "net452",
                    Version = "0.21.1"
                },
                new NuSpecDependency
                {
                    Id = "Cake.Core",
                    TargetFramework = "net46",
                    Version = "0.22.0"
                },
                new NuSpecDependency
                {
                    Id = "Minio.NetCore",
                    TargetFramework = "netstandard1.6",
                    Version = "1.0.7"
                },
                new NuSpecDependency
                {
                    Id = "Minio",
                    TargetFramework = "net452",
                    Version = "1.0.7"
                },
                new NuSpecDependency
                {
                    Id = "Minio",
                    TargetFramework = "net46",
                    Version = "1.0.7"
                }
            },
            BasePath = "./src/Cake.Minio/bin/release",
            OutputDirectory = "./nuget"
        };

        NuGetPack(nuGetPackSettings);
    });

Task("Update-Appveyor-Build-Version")
    .Does(() =>
    {
        if (AppVeyor.IsRunningOnAppVeyor)
        {
            AppVeyor.UpdateBuildVersion(version + string.Concat("+", buildNumber));
        }
        else
        {
            Information("Not running on AppVeyor");
        }
    });

Task("Publish")
    .Does(() =>
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Could not resolve Nuget API key.");
        };

        var packagePath = "./nuget/" + appName + "." + version + ".nupkg";
        Information("Publishing: {0}", packagePath);
        NuGetPush(packagePath, new NuGetPushSettings
        {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = apiKey
        });
    });

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Pack");

Task("AppVeyor")
    .IsDependentOn("Default")
    .IsDependentOn("Update-Appveyor-Build-Version")
    .IsDependentOn("Publish");

RunTarget(target);
