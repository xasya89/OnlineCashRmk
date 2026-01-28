using System.Diagnostics;
using System.IO.Compression;

namespace OnlineCashRmk.Launcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const string CashFolderName = "online-cash";

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var config = LoadingConfig.LoadConfig();

                labelStatus.Text = "Проверка обновлений...";
                var appPath = Path.Combine(Application.StartupPath, "online-cash", config.AppName);
                if (!File.Exists(appPath))
                {
                    throw new FileNotFoundException($"Основное приложение не найдено: {appPath}");
                }

                var currentVersion = GetLocalVersion(appPath);
                var serverVersion = await FetchServerVersionAsync(config.VersionUrl);

                if (serverVersion <= currentVersion)
                {
                    labelStatus.Text = "Обновлений не найдено.";
                    await Task.Delay(1500);
                    LaunchAppAndExit(appPath);
                    return;
                }

                labelStatus.Text = "Загрузка обновления...";
                var tempZip = Path.Combine(Path.GetTempPath(), "OnlineCash_update.zip");
                var tempExtract = Path.Combine(Path.GetTempPath(), "OnlineCash_update");

                Directory.CreateDirectory(tempExtract);

                await DownloadFileAsync(config.DownloadUrl, tempZip, progress =>
                {
                    progressBar.Value = progress;
                });

                labelStatus.Text = "Распаковка...";
                ZipFile.ExtractToDirectory(tempZip, tempExtract, true);

                labelStatus.Text = "Установка...";
                ReplaceAppFiles(tempExtract, Path.Combine(Application.StartupPath, CashFolderName), skipFiles: new[] { "Updater.exe", "appsettings.json" });

                labelStatus.Text = "Запуск приложения...";
                LaunchAppAndExit(appPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private Version GetLocalVersion(string appPath)
        {
            return Version.TryParse(FileVersionInfo.GetVersionInfo(appPath).FileVersion, out var v) ? v : new Version(0, 0, 0);
        }

        private async Task<Version> FetchServerVersionAsync(string versionUrl)
        {
            using var client = new HttpClient();
            var text = await client.GetStringAsync(versionUrl);
            return Version.Parse(text.Trim());
        }

        private async Task DownloadFileAsync(string url, string filePath, Action<int> onProgress)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength ?? -1L;
            using var contentStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = File.Create(filePath);

            var buffer = new byte[8192];
            var totalRead = 0L;
            int bytesRead;

            while ((bytesRead = await contentStream.ReadAsync(buffer)) > 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                totalRead += bytesRead;

                if (totalBytes > 0)
                {
                    var progress = (int)((double)totalRead / totalBytes * 100);
                    onProgress(Math.Min(progress, 100));
                }
            }
        }

        private void ReplaceAppFiles(string sourceDir, string targetDir, string[] skipFiles)
        {
            var skipSet = new HashSet<string>(skipFiles, StringComparer.OrdinalIgnoreCase);

            foreach (var file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                var relativePath = file.Substring(sourceDir.Length + 1);
                var targetPath = Path.Combine(targetDir, relativePath);
                var fileName = Path.GetFileName(targetPath);

                if (skipSet.Contains(fileName))
                    continue;

                Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
                File.Copy(file, targetPath, overwrite: true);
            }
        }

        private void LaunchAppAndExit(string appPath)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = appPath,
                UseShellExecute = true
            });
            Application.Exit();
        }
    }
}
