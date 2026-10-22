using DsDemo.Data;
using DsDemo.Data.Entities;
using DsDemo.Data.EventObjects.Graph;
using Microsoft.EntityFrameworkCore;

namespace DsDemo.Services;

public class GraphService : IGraphService
{
    private ApplicationDbContext _context;

    public GraphService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Station>> GetAllStations()
        => await _context.Stations.AsNoTracking().ToListAsync();
    

    public async Task<List<Station>> GetPathBetweenStations(int departure, int destination)
    {
        var stations = await _context.Stations.AsNoTracking().ToListAsync();
        var ways = await _context.Ways.AsNoTracking().ToListAsync();

        var graph = new Graph<int>();

        var nodes = new List<Node<int>>();

        foreach (var s in stations)
            nodes.Add(graph.AddNode(s.Id));

        foreach (var way in ways)
        {
            var node1 = nodes.First(a => a.Value == way.From);
            var node2 = nodes.First(a => a.Value == way.To);
            graph.AddEdge(node1,node2,1);
        }

        var dep = graph.Nodes.First(a => a.Value == departure);
        var des = graph.Nodes.First(a => a.Value == destination);

        var path = graph.GetShortestPathDijkstra(dep,des);

        var directPath = graph.ToDirectPath(path);

        var result = new List<Station>();

        foreach (var p in directPath)
            result.Add(stations.First(a => a.Id == p));

        return result;
    }
}
