using MOPBUILD;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using System.Collections.Generic;

using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

partial class Build
{
    List<AbsolutePath> PluginsSourceDirectory => new List<AbsolutePath>
    {
        SourceDirectory / "MOP.Remote",
    };

    List<Project> PluginsProjects => new List<Project>
    {
        MopSolution.GetProject("MOP.Remote"),
    };
    
    AbsolutePath PluginsOutputDirectory => OutputDirectory / "MOPHost" / "Plugins";

    Target PluginsClean => _ => _
        .Executes(() =>
        {
            PluginsSourceDirectory.ForEach(p => 
                p.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory));
            EnsureCleanDirectory(PluginsOutputDirectory);
        });

    Target PluginsRestore => _ => _
        .DependsOn(PluginsClean)
        .Executes(() =>
        {
            PluginsProjects.ForEach(p =>
            {
                Logger.Info("Restoring plugin: " + p.Name);
                DotNetRestore(s => s
                    .SetMopRuntime(Runtime)
                    .SetProjectFile(p));
            });
        });

    public Target PluginsQuick => _ => _
        .DependsOn(PluginsClean)
        .Executes(() =>
        {
            PluginsProjects.ForEach(p =>
            {
                Logger.Info("Compiling plugin: " + p.Name);
                DotNetBuild(s => s
                    .SetProjectFile(p)
                    .SetConfiguration(Configuration)
                    .SetMopRuntime(Runtime)
                    .SetOutputDirectory(PluginsOutputDirectory));
            });
        });

    Target PluginsCompile => _ => _
        .DependsOn(PluginsRestore)
        .DependsOn(HostCompile)
        .Executes(() =>
        {
            PluginsProjects.ForEach(p =>
            {
                Logger.Info("Compiling plugin: " + p.Name);
                DotNetBuild(s => s
                    .SetProjectFile(p)
                    .SetConfiguration(Configuration)
                    .SetMopRuntime(Runtime)
                    .SetOutputDirectory(PluginsOutputDirectory)
                    .EnableNoRestore());
            });
        });
}