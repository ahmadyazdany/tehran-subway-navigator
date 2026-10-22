using DsDemo.Services;
using DsDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DsDemo.Controllers;

public class GraphController : Controller
{
    private IGraphService _graphService;

    public GraphController(IGraphService graphService)
    {
        _graphService = graphService;
    }

    [HttpGet("/graph")]
    public async Task<IActionResult> Index()
    {
        ViewBag.Stations = new SelectList(await _graphService.GetAllStations(),"Id","Name");
        return View();
    }

    [HttpPost("/graph")]
    public async Task<IActionResult> Index(StationsViewModel vm)
    {
        ViewBag.Stations = new SelectList(await _graphService.GetAllStations(), "Id", "Name");

        if (!ModelState.IsValid) 
            return View(vm);

        vm.Stations = await _graphService.GetPathBetweenStations(vm.DepartureId, vm.DestinationId);
        return View(vm);
    }
}