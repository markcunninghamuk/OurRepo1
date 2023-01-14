using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ajay.Legend.App.Repositories;
using Ajay.Legend.App.Repositories.Entities;
using Defra.Cdp.Telemetry.Loggers;
using Ajay.Legend.App.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Ajay.Legend.App.Services
{
    /// <summary>
    /// Performs some example functions.
    /// </summary>
    public class SheepService : ISheepService
    {
        private readonly IGenericRepository<Sheep> sheepRepository;
        private readonly IMapper mapper;
        private readonly IStandardLogger logger;

        /// <summary>
        /// Constructor for Example Service.
        /// </summary>
        /// <param name="configuration">Settings from appsettings/usersecrets/AppConfigurationService depending on the environment.</param>
        /// <param name="sheepRepository">sheep repo</param>
        /// <param name="logger">Standard logger (entries get sanitised)</param>
        public SheepService(IConfiguration configuration, IGenericRepository<Sheep> sheepRepository, IMapper mapper, IStandardLogger logger)
        {
            var config = configuration.GetSection("DevOps");
            this.sheepRepository = sheepRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SheepModel>> ListSheepsAsync()
        {
            // Examples for retrieving the list of sheeps
            // ===========================================
            // Filtering example using GetFilteredAsync:
            // var persons = await this.sheepRepository.GetFilteredAsync(x => x.FirstName != null);
            //
            // Filtering and sorting example using GetFilteredAsync:
            // var persons = await this.sheepRepository.GetFilteredAsync(x => x.FirstName != null, order => order.OrderBy(z => z.FirstName));
            //
            // Filtering example using GetAll:
            // var persons = await this.sheepRepository.GetAll().Where(x => x.FirstName != null).OrderBy(z => z.FirstName).ToListAsync();
            //
            // Ordering example using GetAll():
            // var persons = await this.sheepRepository.GetAll().OrderBy(x => x.LastName).ToListAsync();
            //
            var sheeps = await this.sheepRepository.GetAll().OrderBy(z => z.LastName).ThenBy(x => x.FirstName).ToListAsync();
            var sheepModels = this.mapper.Map<IEnumerable<SheepModel>>(sheeps);
            return sheepModels;
        }

        /// <inheritdoc/>
        public async Task<SheepModel> GetSheepAsync(Guid? id)
        {
            var sheep = await this.sheepRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            var sheepModel = this.mapper.Map<SheepModel>(sheep);
            return sheepModel;
        }

        /// <inheritdoc/>
        public async Task DeleteSheepAsync(Guid? id)
        {
            var sheep = await this.sheepRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (sheep != null)
            {
                this.sheepRepository.Delete(sheep);
                await this.sheepRepository.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task SaveSheepAsync(SheepModel model)
        {
            var entity = this.mapper.Map<Sheep>(model);
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
                this.sheepRepository.Insert(entity);
            }
            else
            {
                this.sheepRepository.Update(entity);
            }

            await this.sheepRepository.SaveChangesAsync();
        }
    }
}