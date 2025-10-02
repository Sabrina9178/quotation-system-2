using QuotationSystem2.ApplicationLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.ApplicationLayer.Interfaces
{
    public interface IQuoteQueryService
    {
        public Task<List<PipeDiameterDto>> GetDiameters();
        public Task<List<PipeThicknessDto>> GetThicknessesByDiameterAndProductNameAsync(int diameterID, string productName);
        public Task<decimal> GetPipeUnitPriceByDiameterAndThicknessAsync(int diameterID, int thicknessID);
        public Task<List<ProductDto>> GetProductsBySuffixAsync(string suffix);
        public Task<List<PipeTopDto>> GetPipeTopsByProductAndDiameterAsync(int productID, int diameterID);
        public Task<decimal> GetPipeTopUnitPriceByDiameterAndPipeTopAsync(int diameterID, int pipeTopID);
        public Task<decimal> GetComponentPriceByNameAndDiameterAsync(string componentName, int diameterID);
        public Task<List<PipeDiameterDto>> GetComponentDiametersByNameAsync(string componentName);
        public Task<List<CustomerDto>> GetCustomersAsync();
        public Task<List<StudDto>> GetStudsAsync();
        public Task<int> CreateTranslationGroupAsync();
        public Task<List<OperatorDto>> GetOperatorsAsync();
        public decimal GetFinalPriceByOperatorAndOperandAsync(OperatorDto operato, decimal operand, decimal totalPrice);
        public Task<List<WaterMeterViewDto>> GetWaterMeterRecordsViewAsync(int page, int pageSize);
        public Task<List<SealingPlateViewDto>> GetSealingPlateRecordsViewAsync(int page, int pageSize);
        public Task<int> GetTotalRecordCountAsync<T>() where T : class;
        public Task<WaterMeterEditDto> GetWaterMeterRecordByIdAsync(int id);

        public Task<SealingPlateEditDto> GetSealingPlateRecordByIdAsync(int id);


    }
}
