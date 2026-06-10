using System.Text;
using System.Text.Json;
using InvoicesApp.Lib;

namespace Server.MiddlewareNS.LogNS;

public static class LogMdw
{
  public static async Task Log(HttpContext ctx, Func<Task> next)
  {
    ctx.Request.EnableBuffering();

    object logObj = await BuildObj(ctx);

    string dir = Path.Combine(
        Directory.GetCurrentDirectory(),
        ".logging"
    );
    Directory.CreateDirectory(dir);

    string filePath = Path.Combine(
        dir,
        $"{DateTime.UtcNow:yyyy-MM-dd}.jsonl"
    );

    string json = JsonParserLib.Stringify(logObj);

    await File.AppendAllTextAsync(
        filePath,
        json + Environment.NewLine
    );

    await next();
  }

  private static async Task<string> ReadBody(HttpRequest? req)
  {
    string body = "";

    if (
        req?.ContentType?
            .StartsWith("multipart/form-data") == true
    )
    {
      return "multipart body skipped";
    }

    if (req?.ContentLength > 0)
    {
      using StreamReader reader = new(
          req.Body,
          Encoding.UTF8,
          leaveOpen: true
      );

      body = await reader.ReadToEndAsync();

      req.Body.Position = 0;
    }

    return body;
  }

  private static async Task<object> BuildObj(HttpContext ctx)
  {
    HttpRequest req = ctx.Request;

    string body = await ReadBody(req);

    object? bodyObj = null;

    if (!string.IsNullOrWhiteSpace(body))
    {
      try
      {
        bodyObj = JsonParserLib.Parse<object>(body);
      }
      catch
      {
        bodyObj = body;
      }
    }

    return new
    {
      Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
      req.Method,
      Path = req.Path.ToString(),
      Query = req.Query.ToDictionary(
            q => q.Key,
            q => q.Value.ToString()
        ),
      Headers = req.Headers.ToDictionary(
            h => h.Key,
            h => h.Value.ToString()
        ),
      RouteParams = ctx.Request.RouteValues.ToDictionary(
            r => r.Key,
            r => r.Value?.ToString()
        ),
      Body = bodyObj
    };
  }
}