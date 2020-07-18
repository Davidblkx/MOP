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
    AbsolutePath HostSourceDirectory => SourceDirectory / "MOP.Host";
    AbsolutePath HostOutputDirectory => OutputDirectory / "MOPHost";
    Project MopHost => MopSolution.GetProject("MOP.Host");

    Target HostClean => _ => _
        .Executes(() =>
        {
            HostSourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(HostOutputDirectory);
        });

    Target HostRestore => _ => _
        .DependsOn(HostClean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(MopHost));
        });

    Target HostCompile => _ => _
        .DependsOn(HostRestore)
        .DependsOn(CoreCompile)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(MopHost)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(HostOutputDirectory)
                .EnableNoRestore());
        });
}