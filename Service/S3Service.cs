using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Collections.Generic;
using System.Threading.Tasks;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Service(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["AWS:S3BucketName"];
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = fileName,
            BucketName = _bucketName,
            ContentType = "application/pdf"
        };

        var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"{fileName} uploaded Successfully";
    }

    public async Task<List<string>> GetFilesByCoursePrefixAsync(string coursePrefix, string bucketName)
    {
        var fileList = new List<string>();
        var request = new ListObjectsV2Request
        {
            BucketName = bucketName,
            Prefix = coursePrefix // Filter by prefix to match course
        };

        try
        {
            ListObjectsV2Response response;
            do
            {
                response = await _s3Client.ListObjectsV2Async(request);
                foreach (var s3Object in response.S3Objects)
                {
                    fileList.Add(s3Object.Key);
                }
                request.ContinuationToken = response.NextContinuationToken;
            }
            while (response.IsTruncated);
        }
        catch (AmazonS3Exception ex)
        {
      
            throw new Exception($"S3 error: {ex.Message}");
        }

        return fileList;
    }

    public string GeneratePreSignedUrl(string key, string bucketName)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = key,
            Expires = DateTime.Now.AddMinutes(15), // URL expiration time
            Verb = HttpVerb.GET
        };

        return _s3Client.GetPreSignedURL(request);
    }
}
