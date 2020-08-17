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
    AbsolutePath TerminalSourceDirectory => SourceDirectory / "MOP.Terminal";
    AbsolutePath TerminalOutputDirectory => OutputDirectory / "MOPTerminal";
    Project MopTerminal => MopSolution.GetProject("MOP.Terminal");

    Target TerminalClean => _ => _
        .Executes(() =>
        {
            TerminalSourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(TerminalOutputDirectory);
        });

    Target TerminalRestore => _ => _
        .DependsOn(TerminalClean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetMopRuntime(Runtime)
                .SetProjectFile(MopTerminal));
        });

    Target TerminalCompile => _ => _
        .DependsOn(TerminalRestore)
        .DependsOn(CoreCompile)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(MopTerminal)
                .SetConfiguration(Configuration)
                .SetMopRuntime(Runtime)
                .SetOutputDirectory(TerminalOutputDirectory));
        });
}