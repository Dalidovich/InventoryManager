using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL;
using InventoryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.HostedServices
{
    public class EnterSeedDataHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public EnterSeedDataHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var categoryService = scope.ServiceProvider.GetRequiredService<IService<InventoryCategory>>();
            if (!await context.InventoryCategorys.AnyAsync())
            {
                var category0 = (await categoryService.CreateEntityAsync(new InventoryCategory() { Name = "Offic" })).Data;
                var category1 = (await categoryService.CreateEntityAsync(new InventoryCategory() { Name = "Kitchen" })).Data;
                var category2 = (await categoryService.CreateEntityAsync(new InventoryCategory() { Name = "Market" })).Data;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}