using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Runtime configuration: win-x64, linux-x64, osx-x64")]
    readonly string Runtime = "";

    [Solution] readonly Solution MopSolution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Compile => _ => _
        .DependsOn(HostCompile)
        .DependsOn(TerminalCompile)
        .DependsOn(PluginsCompile)
        .Executes(() => Logger.Success("DONE"));
}
