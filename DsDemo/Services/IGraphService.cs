using DsDemo.Data.Entities;

namespace DsDemo.Services;

public interface IGraphService
{
    Task<List<Station>> GetAllStations();
    Task<List<Station>> GetPathBetweenStations(int departure, int destination);
}
