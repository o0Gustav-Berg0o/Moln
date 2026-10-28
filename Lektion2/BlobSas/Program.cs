using Azure.Storage.Blobs;

internal class Program
{
    private static async Task Main(string[] args)
    {

        var accountUri = "https://<kontonamn>.blob.core.windows.net";
        var container = "bob";
        var blobName = "aaa.docx";

        var sas = "<sas-token>";

        var url = $"{accountUri}/{container}/{Uri.EscapeDataString(blobName)}?{sas}";

        var blob = new BlobClient(new Uri(url));

        var content = await blob.DownloadContentAsync();
        Console.WriteLine(content.Value.Content.ToString());
    }
}