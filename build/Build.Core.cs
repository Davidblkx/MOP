using MOPBUILD;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

partial class Build
{
    AbsolutePath CoreSourceDirectory => SourceDirectory / "MOP.Core";
    AbsolutePath InfraSourceDirectory => SourceDirectory / "MOP.Infra";
    Project MopCore => MopSolution.GetProject("MOP.Core");
    Project MopInfra => MopSolution.GetProject("MOP.Infra");

    Target CoreClean => _ => _
        .Executes(() =>
        {
            CoreSourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
        });

    Target InfraClean => _ => _
        .Executes(() =>
        {
            InfraSourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
        });

    Target CoreRestore => _ => _
        .DependsOn(CoreClean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetMopRuntime(Runtime)
                .SetProjectFile(MopCore));
        });

    Target InfraRestore => _ => _
        .DependsOn(InfraClean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetMopRuntime(Runtime)
                .SetProjectFile(MopInfra));
        });

    Target CoreCompile => _ => _
        .DependsOn(CoreRestore)
        .DependsOn(InfraCompile)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(MopCore)
                .SetMopRuntime(Runtime)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target InfraCompile => _ => _
        .DependsOn(InfraRestore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(MopInfra)
                .SetMopRuntime(Runtime)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });
}
