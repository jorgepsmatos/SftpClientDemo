namespace SftpClient
{
    internal static class Program
    {
        private static void Main()
        {
            const string Host = "localhost";
            const string User = "user";
            const string Password = "12345";

            var sftp = new Sftp("/ftproot/");

            sftp.Upload("test.txt", Host, User, Password);

            sftp.ListDirectory(string.Empty, Host, User, Password);

            sftp.Download("test.txt", Host, User, Password);

            sftp.Delete("test.txt", Host, User, Password);
        }
    }
}
