#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

namespace Stargaze.Tools
{
    public static class ProjectFolderHelper
    {
        // Credits to user 'wappenull' on StackOverflow for this function.
        public static bool GetOpenFolderDirectory(out string path)
        {
            var _tryGetActiveFolderPath = typeof(ProjectWindowUtil).GetMethod( "TryGetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic );

            object[] args = new object[] { null };
            bool found = (bool)_tryGetActiveFolderPath.Invoke( null, args );
            path = (string)args[0];

            return found;
        }
    }
}
#endif