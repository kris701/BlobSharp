
<p align="center">
    <img src="https://github.com/user-attachments/assets/90264492-f2d1-475c-922a-a35e56629c99" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/BlobSharp/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/BlobSharp/actions/workflows/dotnet-desktop.yml)
![Nuget](https://img.shields.io/nuget/v/BlobSharp)
![Nuget](https://img.shields.io/nuget/dt/BlobSharp)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/BlobSharp/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/BlobSharp)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--8.0-green)


# BlobSharp

BlobSharp is a simple managed interface to communicate with [Azure Blob Storage accounts](https://azure.microsoft.com/en-us/products/storage/blobs/?ef_id=_k_Cj0KCQjww5u2BhDeARIsALBuLnN_Sexqki1LbNjC2Ilf9sVSfM4b-3MUivbJlEJb3Rall4hkPDsFkVAaAsVpEALw_wcB_k_&OCID=AIDcmmeauvx05c_SEM__k_Cj0KCQjww5u2BhDeARIsALBuLnN_Sexqki1LbNjC2Ilf9sVSfM4b-3MUivbJlEJb3Rall4hkPDsFkVAaAsVpEALw_wcB_k_&gad_source=1&gclid=Cj0KCQjww5u2BhDeARIsALBuLnN_Sexqki1LbNjC2Ilf9sVSfM4b-3MUivbJlEJb3Rall4hkPDsFkVAaAsVpEALw_wcB).
You can download, upload, delete, etc files on your blob storage account with this package.

As an example, one can do the following to download a file from your blob storage:
```csharp
var client = new BlobStorageClient("<--Connection String Here-->");
using (var stream = await client.DownloadAsStreamAsync("somedir", "file"))
{
    // Do something with the file.
}
```

The package is available on the [NuGet Package Manager](https://www.nuget.org/packages/BlobSharp/).