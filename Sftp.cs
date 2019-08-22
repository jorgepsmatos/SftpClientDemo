namespace SftpClient
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Renci.SshNet;

    internal class Sftp
    {
        private string root;

        public Sftp(string root)
        {
            this.root = root;
        }

        public void Upload(string fileToUpload, string host, string username, string password)
        {
            try
            {
                using (var sftpClient = new SftpClient(host, username, password))
                using (var fs = new FileStream(fileToUpload, FileMode.Open))
                {
                    sftpClient.Connect();

                    sftpClient.UploadFile(
                        fs,
                        root + Path.GetFileName(fileToUpload),
                        uploaded =>
                             {
                                 Console.WriteLine($"Uploaded {(double)uploaded / fs.Length * 100}% of the file.");
                             });

                    sftpClient.Disconnect();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ListDirectory(string remoteDirectory, string host, string username, string password)
        {
            try
            {
                using (var sftpClient = new SftpClient(host, username, password))
                {
                    sftpClient.Connect();

                    var files = sftpClient.ListDirectory(root + remoteDirectory);
                    files = files.Where(f => !Regex.IsMatch(f.Name, @"^\.+"));

                    foreach (var file in files)
                    {
                        Console.WriteLine(file.Name);
                    }

                    sftpClient.Disconnect();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Download(string fileToDownload, string host, string username, string password)
        {
            try
            {
                using (var sftpClient = new SftpClient(host, username, password))
                using (var fs = new FileStream(Path.GetFileName(fileToDownload), FileMode.OpenOrCreate))
                {
                    sftpClient.Connect();

                    sftpClient.DownloadFile(
                        root + fileToDownload,
                        fs,
                        downloaded =>
                            {
                                Console.WriteLine($"Downloaded {(double)downloaded / fs.Length * 100}% of the file.");
                            });

                    sftpClient.Disconnect();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Delete(string fileToDelete, string host, string username, string password)
        {
            try
            {
                using (var sftpClient = new SftpClient(host, username, password))
                {
                    sftpClient.Connect();

                    sftpClient.DeleteFile(root + fileToDelete);

                    sftpClient.Disconnect();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}