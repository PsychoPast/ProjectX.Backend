using System.Threading.Tasks;

namespace ProjectX.Backend.DatabaseCollections
{
    public interface ILevelManager
    {
        Task AddExperienceAsync(ulong amt);
        Task IncrementLevelAsync();
    }
}