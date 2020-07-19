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
    Project MopCore => MopSolution.GetProject("MOP.Core");

    Target CoreClean => _ => _
        .Executes(() =>
        {
            CoreSourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
        });

    Target CoreRestore => _ => _
        .DependsOn(CoreClean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetMopRuntime(Runtime)
                .SetProjectFile(MopCore));
        });

    Target CoreCompile => _ => _
        .DependsOn(CoreRestore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(MopCore)
                .SetMopRuntime(Runtime)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });
}
