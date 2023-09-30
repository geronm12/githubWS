using System.Diagnostics;

namespace GithubWS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static string _basePath = @"C:\\users\gerol\desktop";
        private static char _baseChar = '\\';
        private Process _process;
        private static object _lock = new object();
        private ProcessStartInfo _starInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        public Process Process
        {
            get
            {
                lock (_lock)
                {
                    if (_process == null)
                    {
                        _process = new Process()
                        {
                            StartInfo = _starInfo
                        };
                    }

                    return _process;
                }
            }
        }

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lastCreatedDirectory = new DirectoryInfo(_basePath);
            string path = lastCreatedDirectory.GetNewestDirectoryInFolder();

            while (!stoppingToken.IsCancellationRequested)
            {
                _process.Start();
                _process.StandardInput.WriteLine(@"cd C:\");
                _process.StandardInput.WriteLine(@$"cd ${_basePath}${_baseChar}${path}");
                _process.StandardInput.WriteLine("git status");
                _process.StandardInput.WriteLine("git add .");
                _process.StandardInput.WriteLine("git commit -m \"commit desde worker service\"");
                _process.StandardInput.WriteLine("git push origin main");
                _process.WaitForExit(1000);
                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}