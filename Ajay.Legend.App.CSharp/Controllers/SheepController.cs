using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ajay.Legend.App.Services;
using Ajay.Legend.App.Models;
using System.Threading.Tasks;
using System;

namespace Ajay.Legend.App.CSharp.Controllers
{
    /// <summary>
    /// A test controller to handle the test persons page.
    /// </summary>
    [AllowAnonymous]
    public class SheepController : Controller
    {
        /// <summary>
        /// Sheep service.
        /// </summary>
        private readonly ISheepService sheepService;

        /// <summary>
        /// Constructor for sheep service.
        /// </summary>
        /// <param name="sheepService"></param>
        public SheepController(ISheepService sheepService)
        {
            this.sheepService = sheepService;
        }

        /// <summary>
        /// Displays the list of sheeps.
        /// </summary>
        /// <returns>Action result.</returns>
        public async Task<IActionResult> Sheeps()
        {
            var sheeps = await this.sheepService.ListSheepsAsync();
            return this.View(sheeps);
        }

        /// <summary>
        /// Displays the sheep for editing.
        /// </summary>
        /// <param name="id">Sheep id</param>
        /// <returns>Action result.</returns>
        public async Task<IActionResult> EditSheep(Guid? id)
        {
            var sheep = id == null || id == Guid.Empty
                ? new SheepModel()
                : await this.sheepService.GetSheepAsync(id);
            return this.View(sheep);
        }

        /// <summary>
        /// Deletes the sheep.
        /// </summary>
        /// <returns>Action result.</returns>
        public async Task<IActionResult> DeleteSheep(Guid? id)
        {
            await this.sheepService.DeleteSheepAsync(id);
            return this.RedirectToAction("Sheeps");
        }

        /// <summary>
        /// Updates an existing sheep, or creates a new sheep.
        /// </summary>
        /// <param name="model">View model posted from the page.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public async Task<IActionResult> EditSheep(SheepModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.sheepService.SaveSheepAsync(model);
            return this.RedirectToAction("Sheeps");
        }
    }
}
