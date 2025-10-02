using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuotationSystem2.DataAccessLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Models.Contexts;
using QuotationSystem2.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuotationSystem2.DataAccessLayer.Repositories
{
    internal class QuoteQueryRepo : IQuoteQueryRepo
    {
        private readonly QuotationSystem2DBContext _context;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public QuoteQueryRepo(QuotationSystem2DBContext context)
        {
            _context = context;
        }


        public async Task<List<PipeDiameter>> GetPipeDiametersAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _context.PipeDiameters.ToListAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<PipeThickness>> GetPipeThicknessesByDiameterIDAsync(int diameterID)
        {
            await _semaphore.WaitAsync();
            try
            {
                var diameter = await _context.PipeDiameters
                .Include(d => d.PipePrices)
                    .ThenInclude(d => d.Thickness)
                .FirstOrDefaultAsync(d => d.DiameterID == diameterID);

                if (diameter == null)
                    throw new KeyNotFoundException($"PipeDiameter ID not found, ID = {diameterID}");

                // 找到PipePrices中所有 "DiameterID = selectedID" 的 ThicknessID
                var prices = diameter.PipePrices
                    .Distinct()
                    .ToList();

                List<PipeThickness> thicknesses = new List<PipeThickness>();
                foreach (var price in prices)
                {
                    if (price.Thickness.DisplayName.EndsWith("t"))
                        thicknesses.Add(price.Thickness);
                }

                return thicknesses;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<PipeThickness>> GetPipeThicknessByDisplayNameAsync(int diameterID, string displayName)
        {
            await _semaphore.WaitAsync();
            try
            {
                List<PipeThickness> thicknesses = new();
                var thickness = await _context.PipeThicknesses
                    .FirstOrDefaultAsync(d => d.DisplayName == displayName);
                if (thickness != null)
                {
                    thicknesses.Add(thickness);
                }
                return thicknesses;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<decimal> GetPipePriceByDiameterIDAndThicknessIDAsync(int diameterID, int thicknessID)
        {
            await _semaphore.WaitAsync();
            try
            {
                var price = await _context.PipePrices
                .Where(p => p.DiameterID == diameterID && p.ThicknessID == thicknessID)
                .FirstOrDefaultAsync();

                if (price == null)
                    throw new KeyNotFoundException($"Price not found, diameterID = {diameterID}, thicknessID = {thicknessID}");

                return price.PipeUnitPrice;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<Product>> GetProductsBySuffixAsync(string suffix)
        {
            await _semaphore.WaitAsync();
            try
            {
                var products = await _context.Products
                .Include(p => p.Translation)
                    .ThenInclude(t => t.Translations)
                .Where(p => p.ProductName.EndsWith(suffix))
                .ToListAsync();
                if (products == null || !products.Any())
                    throw new KeyNotFoundException($"Product with suffix '{suffix}' not found.");
                return products;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<PipeTop>> GetPipeTopsByProductIDAndDiameterIDAsync(int productID, int diameterID)
        {
            await _semaphore.WaitAsync();
            try
            {
                var pipeTopID_product = await _context.ProductPipeTopMappings
                .Where(pt => pt.ProductID == productID)
                .Select(pt => pt.PipeTopID)
                .ToListAsync();

                var pipeTopIDs_diameter = await _context.PipeTopPrices
                    .Where(pt => pt.DiameterID == diameterID)
                    .Select(ptp => ptp.PipeTopID)
                    .ToListAsync();

                // 取出兩個列表的交集
                var pipeTopIDs = pipeTopID_product.Intersect(pipeTopIDs_diameter).ToList();

                // 在PipeTop中, 把所有PipeTopID = pipeTopIDs的資料都找出來
                var pipeTops = await _context.PipeTops
                    .Include(p => p.Translation)
                        .ThenInclude(t => t.Translations)
                    .Include(m => m.PricingMethodNavigation)
                        .ThenInclude(t => t.Translation)
                        .ThenInclude(t => t.Translations)
                    .Where(pt => pipeTopIDs.Contains(pt.PipeTopID))
                    .ToListAsync();

                if (pipeTops == null || !pipeTops.Any())
                    throw new KeyNotFoundException($"No PipeTop found for productID = {productID}.");

                return pipeTops;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<decimal> GetPipeTopUnitPriceByDiameterIDAndPipeTopID(int diameterID, int pipeTopID)
        {
            await _semaphore.WaitAsync();
            try
            {
                var priceEntry = await _context.PipeTopPrices
                .FirstOrDefaultAsync(p => p.DiameterID == diameterID && p.PipeTopID == pipeTopID);

                if (priceEntry == null)
                    throw new KeyNotFoundException($"PipeTop price not found for diameter ID {diameterID} and PipeTop ID {pipeTopID}.");

                return priceEntry.Price;
            }
            finally
            {
                _semaphore.Release();
            }
        }


        public async Task<int> GetComponentIDByNameAsync(string componentName)
        {
            await _semaphore.WaitAsync();
            try
            {
                var component = await _context.Components
                .Include(p => p.Translation)
                    .ThenInclude(t => t.Translations)
                .FirstOrDefaultAsync(c => c.ComponentName == componentName);
                if (component == null)
                    throw new KeyNotFoundException($"Component with name '{componentName}' not found.");
                return component.ComponentID;

            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<int> GetComponentIDByNamePrivateAsync(string componentName)
        {
            // 重複呼叫_semaphore.WaitAsync();, 會造成死鎖 (deadlock)
            var component = await _context.Components
            .Include(p => p.Translation)
                .ThenInclude(t => t.Translations)
            .FirstOrDefaultAsync(c => c.ComponentName == componentName);
            if (component == null)
                throw new KeyNotFoundException($"Component with name '{componentName}' not found.");
            return component.ComponentID;
        }

        public async Task<decimal> GetComponentPriceByNameAndDiameterIDAsync(string componentName, int diameterID)
        {
            await _semaphore.WaitAsync();
            try
            {
                var componentID = await GetComponentIDByNamePrivateAsync(componentName);
                var price = await _context.ComponentPrices
                    .Where(cp => cp.ComponentID == componentID && cp.DiameterID == diameterID)
                    .Select(p => p.Price)
                    .FirstOrDefaultAsync();
                if (price == 0) // Assuming 0 means not found
                    throw new KeyNotFoundException($"Component price not found for component '{componentName}' and diameter ID {diameterID}.");
                return price;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<PipeDiameter>> GetComponentDiametersByNameAsync(string componentName)
        {
            await _semaphore.WaitAsync();
            try
            {
                var componentID = await GetComponentIDByNamePrivateAsync(componentName); //我的城市
                var diameters = await _context.ComponentPrices
                .Where(cp => cp.ComponentID == componentID)
                .Select(cp => cp.Diameter)
                .Distinct()
                .ToListAsync();
                if (diameters == null || !diameters.Any())
                    throw new KeyNotFoundException($"No diameters found for component '{componentName}'.");
                return diameters;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var customers = await _context.Customers
                .Include(p => p.Translation)
                    .ThenInclude(t => t.Translations)
                .ToListAsync();
                if (customers == null || !customers.Any())
                    throw new KeyNotFoundException("No customers found.");
                return customers;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<Stud>> GetStudsAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var studs = await _context.Studs
                .ToListAsync();
                if (studs == null || !studs.Any())
                    throw new KeyNotFoundException("No studs found.");
                return studs;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<Operator>> GetOperatorsAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var operators = await _context.Operators
                .ToListAsync();
                if (operators == null || !operators.Any())
                    throw new KeyNotFoundException("No operators found.");
                return operators;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<int> CreateTranslationGroupAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var translationGroup = new TranslationGroup
                {
                    Dummy = ""
                };

                var translationGroupDb = _context.TranslationGroups.Add(translationGroup);
                await _context.SaveChangesAsync();
                return translationGroupDb.Entity.TranslationID;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<PipePrice>> GetPipePricesAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _context.PipePrices
                .ToListAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<PipeTopPrice>> GetPipeTopPricesAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _context.PipeTopPrices
                .ToListAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var products = await _context.Products
                .Include(p => p.Translation)
                    .ThenInclude(t => t.Translations)
                .ToListAsync();
                if (products == null || !products.Any())
                    throw new KeyNotFoundException("No products found.");
                return products;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<ComponentPrice>> GetComponentPricesAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var componentPrices = await _context.ComponentPrices
                .ToListAsync();
                return componentPrices;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<WaterMeterRecord>> GetWaterMeterRecordsViewAsync(int page, int pageSize)
        {
            await _semaphore.WaitAsync();

            try
            {
                var records = await _context.WaterMeterRecords
                    .Include(r => r.PipeDiameter)
                    .Include(r => r.PipeThickness)
                    .Include(r => r.PipeTop)
                        .ThenInclude(r => r.Translation)
                            .ThenInclude(t => t.Translations)
                    .Include(r => r.WaterMeterRecordNipples)
                        .ThenInclude(r => r.NippleDiameter)
                    .Include(r => r.WaterMeterRecordSockets)
                        .ThenInclude(r => r.SocketDiameter)
                    .Include(r => r.Customer)
                        .ThenInclude(c => c.Translation)
                            .ThenInclude(t => t.Translations)
                    .Include(r => r.OperatorNavigation)
                    .Include(r => r.Employee)
                    .AsNoTracking()
                    .OrderByDescending(x => x.Date)
                    .Skip((page - 1) * 50)
                    .Take(50)
                    .ToListAsync();
                if (records == null)
                    throw new KeyNotFoundException("No water meter records found.");
                return records;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<SealingPlateRecord>> GetSealingPlateRecordsViewAsync(int page, int pageSize)
        {
            await _semaphore.WaitAsync();
            try
            {
                var records = await _context.SealingPlateRecords
                    .Include(r => r.Product)
                        .ThenInclude(r => r.Translation)
                            .ThenInclude(t => t.Translations)
                    .Include(r => r.PipeDiameter)
                    .Include(r => r.PipeThickness)
                    .Include(r => r.PipeTop)
                        .ThenInclude(r => r.Translation)
                            .ThenInclude(t => t.Translations)
                    .Include(r => r.SocketDiameter)
                    .Include(r => r.Customer)
                        .ThenInclude(c => c.Translation)
                            .ThenInclude(t => t.Translations)
                    .Include(r => r.OperatorNavigation)
                    .Include(r => r.Employee)
                    .AsNoTracking()
                    .OrderByDescending(x => x.Date)
                    .Skip((page - 1) * 50)
                    .Take(50)
                    .ToListAsync();
                if (records == null)
                    throw new KeyNotFoundException("No sealing plate records found.");
                return records;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<int> GetTotalRecordCountAsync<TEntity>() where TEntity : class
        {
            {
                await _semaphore.WaitAsync();
                try
                {
                    var dbSet = _context.Set<TEntity>();
                    return await dbSet.CountAsync();
                }
                finally
                {
                    _semaphore.Release();
                }
            }

        }

        public async Task<TEntity> GetRecordByIdAsync<TEntity>(int id) where TEntity : class
        {
            await _semaphore.WaitAsync();
            try
            {
                var entity = await _context.Set<TEntity>()
                    .FindAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException($"Record of type {typeof(TEntity).Name} with id {id} not found.");
                return entity;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<WaterMeterRecord> GetWaterMeterRecordAsync(int id)
        {
            await _semaphore.WaitAsync();
            try
            {
                var entity = await _context.WaterMeterRecords
                    .Include(r => r.WaterMeterRecordSockets)
                    .Include(r => r.WaterMeterRecordNipples)
                    .Where(r => r.WaterMeterID == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                    throw new KeyNotFoundException($"Record of WaterMeterRecords with id {id} not found.");
                return entity;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
