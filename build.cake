#addin nuget:?package=Cake.Incubator&version=8.0.0

var target = Argument("target", "Build");

var workflow = BuildSystem.GitHubActions.Environment.Workflow;
var buildId = workflow.RunNumber;
var tag = workflow.RefType == GitHubActionsRefType.Tag ? workflow.RefName : null;

Task("Build")
    .Does(ctx =>
    {
        var settings = new DotNetBuildSettings
        {
            Configuration = "Release",
            MSBuildSettings = new DotNetMSBuildSettings()
        };

        if (tag != null) 
        {
            settings.MSBuildSettings.Version = tag.TrimStart('v');
        }
        else if (buildId != 0)
        {
            settings.MSBuildSettings.VersionSuffix = "ci." + buildId;
        }

        DotNetBuild(".", settings);
    });

RunTarget(target);
