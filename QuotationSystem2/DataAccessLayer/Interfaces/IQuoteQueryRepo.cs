using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotationSystem2.DataAccessLayer.Models;
using QuotationSystem2.DataAccessLayer.Repositories;

namespace QuotationSystem2.DataAccessLayer.Interfaces
{
    internal interface IQuoteQueryRepo
    {
        public Task<List<PipeDiameter>> GetPipeDiametersAsync();
        public Task<List<PipeThickness>> GetPipeThicknessesByDiameterIDAsync(int diameterID);
        public Task<List<PipeThickness>> GetPipeThicknessByDisplayNameAsync(int diameterID, string displayName);
        public Task<decimal> GetPipePriceByDiameterIDAndThicknessIDAsync(int diameterID, int thicknessID);
        public Task<List<Product>> GetProductsBySuffixAsync(string suffix);
        public Task<List<PipeTop>> GetPipeTopsByProductIDAndDiameterIDAsync(int productID, int diameterID);
        public Task<decimal> GetPipeTopUnitPriceByDiameterIDAndPipeTopID(int diameterID, int pipeTopID);
        public Task<decimal> GetComponentPriceByNameAndDiameterIDAsync(string componentName, int diameterID);
        public Task<List<PipeDiameter>> GetComponentDiametersByNameAsync(string componentName);
        public Task<List<Customer>> GetCustomersAsync();
        public Task<List<Stud>> GetStudsAsync();
        public Task<int> CreateTranslationGroupAsync();
        public Task<List<Operator>> GetOperatorsAsync();
        public Task<List<PipePrice>> GetPipePricesAsync();
        public Task<List<PipeTopPrice>> GetPipeTopPricesAsync();
        public Task<List<Product>> GetProductsAsync();
        public  Task<List<ComponentPrice>> GetComponentPricesAsync();
        public Task<int> GetComponentIDByNameAsync(string componentName);
        public Task<List<WaterMeterRecord>> GetWaterMeterRecordsViewAsync(int page, int pageSize);
        public Task<List<SealingPlateRecord>> GetSealingPlateRecordsViewAsync(int page, int pageSize);
        public Task<int> GetTotalRecordCountAsync<TEntity>() where TEntity : class;
        public Task<TEntity> GetRecordByIdAsync<TEntity>(int id) where TEntity : class;
        public Task<WaterMeterRecord> GetWaterMeterRecordAsync(int id);
    }
}
