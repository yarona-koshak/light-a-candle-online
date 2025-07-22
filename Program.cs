using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

class Program
{
  static void Main()
  {
    int port = 5000;

    var server = new Server(port);

    Console.WriteLine("The server is running");
    Console.WriteLine($"Main Page: http://localhost:{port}/website/pages/openingPage.html");

    var database = new Database();
     AddStartPictures(database);

    while (true)
    {
      (var request, var response) = server.WaitForRequest();

      Console.WriteLine($"Recieved a request with the path: {request.Path}");

      if (File.Exists(request.Path))
      {
        var file = new File(request.Path);
        response.Send(file);
      }
      else if (request.ExpectsHtml())
      {
        var file = new File("website/pages/404.html");
        response.SetStatusCode(404);
        response.Send(file);
      }
      else
      {
        try
        {
          if (request.Path == "getPicture")
          {
            var pictures = database.Pictures.ToArray();

            response.Send(pictures);
          }
          response.SetStatusCode(405);

          database.SaveChanges();
        }
        catch (Exception exception)
        {
          Log.WriteException(exception);
        }
      }

      response.Close();
    }
  }
  static void AddStartPictures(Database database)
  {
    if (database.IsNewlyCreated())
    {
      var startArticals = new Picture[] {
        new Picture(
          "",
          // קישור של תמונה 
          ""
          // הודעה שאת רוצה שתהיה 
          // תחזרי אל התבנית כמה פעמים שתצרכי 
        )
      };
        for (int i = 0; i < startArticals.Length; i++)
      {
        database.Pictures.Add(startArticals[i]);
      }

      database.SaveChanges();
    }
  }
}


class Database() : DbBase("database")
{
  public DbSet<Picture> Pictures { get; set; } = default!;
}

class Picture( string src, string text)
{
  [Key] public int Id { get; set; } = default!;
  public string Src { get; set; } = src;
  public string Text { get; set; } = text;
}