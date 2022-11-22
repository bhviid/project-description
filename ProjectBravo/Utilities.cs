using System.Net;
using ProjectBravo.Core;

public class Utilities
{


    public static HttpStatusCode StatusConversion(Status status) => status switch
    {
        Status.OK => HttpStatusCode.OK,
        Status.Created => HttpStatusCode.Created,
        Status.Deleted => HttpStatusCode.OK,
        Status.Updated => HttpStatusCode.NoContent,
        Status.NotFound => HttpStatusCode.NotFound,
        Status.Conflict => HttpStatusCode.Conflict,
        _ => HttpStatusCode.BadRequest
    };




    public static void setAttributesNormal(DirectoryInfo dir)
    {
        foreach (var subDir in dir.GetDirectories())
            setAttributesNormal(subDir);
        foreach (var file in dir.GetFiles())
        {
            file.Attributes = FileAttributes.Normal;
        }
    }


    public static void setAttributesNormalLINQ(DirectoryInfo dir) =>
        dir.GetDirectories("*", SearchOption.AllDirectories)
        .Where(dir => !dir.GetDirectories().Any()).ToList()
        .ForEach(subDir => subDir.GetFiles().ToList()
        .ForEach(file => file.Attributes = FileAttributes.Normal));

}


