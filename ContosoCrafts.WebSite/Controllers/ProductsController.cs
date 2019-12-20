using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContosoCrafts.WebSite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
    public ProductsController(JsonFileProductService productService)
    {
      this.ProductService = productService;
    }
    public  JsonFileProductService ProductService { get; }
    [HttpGet]
    public IEnumerable<Product> Get()
    {
      return ProductService.GetProducts();
    }
    // [HttpPatch] "[From body]"
    [Route("Rate")]
    [HttpGet]
    public ActionResult Get(
      [FromQuery] string ProductId,
      int Rating)
    {
      ProductService.AddRating(ProductId, Rating);
      return Ok();
    }
  }
}