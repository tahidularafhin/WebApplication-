using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Services
{
  public class JsonFileProductService
  {
    public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
    {
      WebHostEnvironment = webHostEnvironment;
    }

    public IWebHostEnvironment WebHostEnvironment { get; }

    private string JsonFileName
    {
      get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }//here we are combine three parts (wwwroot,data,
    }

    public IEnumerable<Product> GetProducts()
    {
      using (var jsonFileReader = File.OpenText(JsonFileName))
      {
        string value = jsonFileReader.ReadToEnd();

        return JsonSerializer.Deserialize<Product[]>(value,
            new JsonSerializerOptions
            {
              PropertyNameCaseInsensitive = true
            });
      }
    }

    public void AddRating(string productId, int rating)
    {
      var products = GetProducts();

      if (products.First(x => x.Id == productId).Rating == null)
      {
        products.First(x => x.Id == productId).Ratings = new int[] { rating };
      }
      else
      {
        var ratings = products.First(x => x.Id == productId).Ratings.ToList();
        ratings.Add(rating);
        products.First(x => x.Id == productId).Ratings = ratings.ToArray();
      }

      using (var outputStream = File.OpenWrite(JsonFileName))
      {
        JsonSerializer.Serialize<IEnumerable<Product>>(
            new Utf8JsonWriter(outputStream, new JsonWriterOptions
            {
              SkipValidation = true,
              Indented = true
            }),
            products
        );
      }
    }
  }

}
