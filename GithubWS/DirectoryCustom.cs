namespace GithubWS
{
    public static class DirectoryCustom
    {
        public static string GetNewestDirectoryInFolder(this DirectoryInfo info) 
        {
            var lastCreatedDirectory = info.GetDirectories()
                              .OrderByDescending(dir => dir.CreationTime).First();

            var strings = lastCreatedDirectory.ToString().Split('\\');

            return strings[strings.Length - 1];
        }
    }
}
