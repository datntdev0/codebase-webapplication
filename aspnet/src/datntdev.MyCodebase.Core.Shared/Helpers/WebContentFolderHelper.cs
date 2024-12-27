using System;
using System.IO;
using System.Linq;

namespace datntdev.MyCodebase.Helpers;

/// <summary>
/// This class is used to find root path of the web project in
/// unit tests (to find views) and entity framework core command line commands (to find conn string).
/// </summary>
public static class WebContentDirectoryFinder
{
    public static string CalculateContentRootFolder()
    {
        var currentAssembly = typeof(WebContentDirectoryFinder).Assembly;

        var assemblyDirectoryPath = Path.GetDirectoryName(currentAssembly.Location)
            ?? throw new Exception($"Could not find location of {currentAssembly.FullName} assembly!");

        var directoryInfo = new DirectoryInfo(assemblyDirectoryPath);
        while (!DirectoryContainsSolution(directoryInfo.FullName))
        {
            if (directoryInfo.Parent == null)
            {
                throw new Exception("Could not find content root folder!");
            }

            directoryInfo = directoryInfo.Parent;
        }

        var webMvcFolder = Path.Combine(directoryInfo.FullName, "src", "datntdev.MyCodebase.Web.Mvc");
        if (Directory.Exists(webMvcFolder))
        {
            return webMvcFolder;
        }

        var webHostFolder = Path.Combine(directoryInfo.FullName, "src", "datntdev.MyCodebase.Web.Host");
        if (Directory.Exists(webHostFolder))
        {
            return webHostFolder;
        }

        throw new Exception("Could not find root folder of the web project!");
    }

    private static bool DirectoryContainsSolution(string directory, string fileName = null)
    {
        return Directory.GetFiles(directory).Any(filePath =>
        {
            var actualFileName = Path.GetFileName(filePath);
            if (fileName == null) return actualFileName.Contains(".sln");
            else return string.Equals(actualFileName, fileName);
        });
    }
}
