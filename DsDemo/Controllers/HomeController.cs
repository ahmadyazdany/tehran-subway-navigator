using Microsoft.AspNetCore.Mvc;

namespace DsDemo.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
        => View();


    [HttpGet("/Resources")]
    public IActionResult Resources()
        => View();

    [HttpGet("/DownloadSourceCode")]
    public IActionResult DownloadSourceCode()
    => View();
}
