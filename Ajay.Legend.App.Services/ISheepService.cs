using System;
using System.Threading.Tasks;
using Ajay.Legend.App.Models;

namespace Ajay.Legend.App.Services
{
    /// <summary>
    /// Interface defining example service.
    /// </summary>
    public interface ISheepService
    {
        /// <summary>
        /// Enumerates the list of sheeps.
        /// </summary>
        /// <returns>List of sheep records.</returns>
        Task<IEnumerable<SheepModel>> ListSheepsAsync();

        /// <summary>
        /// Gets a sheep's details.
        /// </summary>
        /// <param name="id">sheep id</param>
        /// <returns>sheep details</returns>
        Task<SheepModel> GetSheepAsync(Guid? id);

        /// <summary>
        /// Deletes a sheep.
        /// </summary>
        /// <param name="id">sheep id</param>
        Task DeleteSheepAsync(Guid? id);

        /// <summary>
        /// Saves a sheep.
        /// </summary>
        /// <param name="model">sheep model</param>
        Task SaveSheepAsync(SheepModel model);
    }
}