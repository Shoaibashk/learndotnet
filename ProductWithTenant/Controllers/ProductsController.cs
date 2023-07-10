using Microsoft.AspNetCore.Mvc;
using ProductWithTenant.Data;

namespace ProductWithTenant.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    private readonly AppDbContext _db;

    public ProductsController(ILogger<ProductsController> logger, AppDbContext db)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    // public IActionResult Index()
    // {
    //     return View();
    // }

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View("Error!");
    // } // GET
    [HttpGet]
    public IActionResult GetAll()
    {
        var all = _db.Products.ToList() ?? throw new ArgumentNullException(nameof(_db.Products));
        return Ok(all);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(_db.Products.FirstOrDefault(x => x.Id == id));
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        _db.Products.Add(product);
        _db.SaveChanges();
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _db.Products.Remove(_db.Products.FirstOrDefault(x => x.Id == id)!);
        _db.SaveChanges();
        return NoContent();
    }
}