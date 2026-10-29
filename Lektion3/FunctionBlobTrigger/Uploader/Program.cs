using Azure.Storage.Blobs;

var svc = new BlobServiceClient("UseDevelopmentStorage=true");
var container = svc.GetBlobContainerClient("filagring");
await container.CreateIfNotExistsAsync();
if (args.Length > 0 && args[0] == "upload")
{
    var blob = container.GetBlobClient("samples-workitems/hej.txt");
    await blob.UploadAsync(BinaryData.FromString("Hej från kursen!"), overwrite: true);
    Console.WriteLine("uploaded");
}
else
{
    Console.WriteLine("container ready");
}
