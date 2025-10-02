using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Models;
using QuotationSystem2.DataAccessLayer.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuotationSystem2.ApplicationLayer.Services
{
    internal class QuoteQueryService : IQuoteQueryService
    {
        private readonly IQuoteQueryRepo _quoteQueryRepo;
        private readonly IRecordService _recordService;

        

        public QuoteQueryService(IQuoteQueryRepo quoteQueryRepo, IRecordService recordService)
        {
            _quoteQueryRepo = quoteQueryRepo;
            _recordService = recordService;
        }

        public async Task<List<PipeDiameterDto>> GetDiameters()
        {
            try
            {
                var entities = await _quoteQueryRepo.GetPipeDiametersAsync();
                List<PipeDiameterDto> dtos = new();

                foreach (var diameter in entities)
                {
                    dtos.Add(PipeDiameterMapper.ToDto(diameter));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<List<PipeThicknessDto>> GetThicknessesByDiameterAndProductNameAsync(int diameterID, string productName)
        {
            try
            {
                if (diameterID == 0)
                    throw new ArgumentNullException(nameof(diameterID), "DiameterID cannot be null");

                List<PipeThickness> entities = new();

                if (productName.StartsWith("6445"))
                {
                    entities = await _quoteQueryRepo.GetPipeThicknessByDisplayNameAsync(diameterID, "6445");
                }
                else if (productName.StartsWith("4626"))
                {
                    entities = await _quoteQueryRepo.GetPipeThicknessByDisplayNameAsync(diameterID, "4626");
                }
                else // including "SUSSealingPlate" and "WaterMeter"
                {
                    entities = await _quoteQueryRepo.GetPipeThicknessesByDiameterIDAsync(diameterID);
                }

                List<PipeThicknessDto> dtos = new();
                foreach (var thickness in entities)
                {
                    dtos.Add(PipeThicknessMapper.ToDto(thickness));
                }

                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<decimal> GetPipeUnitPriceByDiameterAndThicknessAsync(int diameterID, int thicknessID)
        {
            try
            {
                if (diameterID == 0 || thicknessID == 0)
                    throw new ArgumentNullException(nameof(diameterID), "DiameterID and ThicknessID cannot be null"); //fix

                return await _quoteQueryRepo.GetPipePriceByDiameterIDAndThicknessIDAsync(diameterID, thicknessID);
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<List<ProductDto>> GetProductsBySuffixAsync(string suffix)
        {
            try
            {
                var entities = await _quoteQueryRepo.GetProductsBySuffixAsync(suffix);
                List<ProductDto> dtos = new();

                foreach (var product in entities)
                {
                    dtos.Add(ProductMapper.ToDto(product));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<List<PipeTopDto>> GetPipeTopsByProductAndDiameterAsync(int productID, int diameterID)
        {
            try
            {
                if (productID == 0 || diameterID == 0)
                    throw new ArgumentNullException(nameof(diameterID), "DiameterID and ProductID cannot be null");

                var entities = await _quoteQueryRepo.GetPipeTopsByProductIDAndDiameterIDAsync(productID, diameterID);

                List<PipeTopDto> dtos = new();
                foreach (var top in entities)
                {
                    dtos.Add(PipeTopMapper.ToDto(top));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<decimal> GetPipeTopUnitPriceByDiameterAndPipeTopAsync(int diameterID, int pipeTopID)
        {
            try
            {
                if (diameterID == 0 || pipeTopID == 0)
                    throw new ArgumentNullException(nameof(diameterID), "DiameterID and PipeTopID cannot be null");
                return await _quoteQueryRepo.GetPipeTopUnitPriceByDiameterIDAndPipeTopID(diameterID, pipeTopID);
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<decimal> GetComponentPriceByNameAndDiameterAsync(string componentName, int diameterID)
        {
            try
            {
                if (diameterID == 0)
                    throw new ArgumentNullException(nameof(diameterID), "DiameterID cannot be 0");

                if (string.IsNullOrWhiteSpace(componentName))
                    throw new ArgumentNullException(nameof(componentName), "Component name cannot be null or empty");

                return await _quoteQueryRepo.GetComponentPriceByNameAndDiameterIDAsync(componentName, diameterID);
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<List<PipeDiameterDto>> GetComponentDiametersByNameAsync(string componentName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(componentName))
                    throw new ArgumentNullException(nameof(componentName), "Component name cannot be null or empty");
                var entities = await _quoteQueryRepo.GetComponentDiametersByNameAsync(componentName);
                List<PipeDiameterDto> dtos = new();

                foreach (var diameter in entities)
                {
                    dtos.Add(PipeDiameterMapper.ToDto(diameter));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<List<CustomerDto>> GetCustomersAsync()
        {
            try
            {
                var customers = await _quoteQueryRepo.GetCustomersAsync();
                List<CustomerDto> dtos = new();

                foreach (var customer in customers)
                {
                    dtos.Add(CustomerMapper.ToDto(customer));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<List<StudDto>> GetStudsAsync()
        {
            try
            {
                var studs = await _quoteQueryRepo.GetStudsAsync();
                List<StudDto> dtos = new();

                foreach (var stud in studs)
                {
                    dtos.Add(StudMapper.ToDto(stud));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<List<OperatorDto>> GetOperatorsAsync()
        {
            try
            {
                var operators = await _quoteQueryRepo.GetOperatorsAsync();
                List<OperatorDto> dtos = new();
                foreach (var op in operators)
                {
                    dtos.Add(OperatorMapper.ToDto(op));
                }

                // Adding an empty operator to the list
                OperatorDto empty = new OperatorDto()
                {
                    OperatorID = 0,
                    OperatorName = ""
                };
                dtos.Add(empty);

                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public decimal GetFinalPriceByOperatorAndOperandAsync(OperatorDto operato, decimal operand, decimal totalPrice)
        {
            try
            {
                switch (operato.OperatorName)
                {
                    case "Add":
                        return totalPrice + operand;
                    case "Subtract":
                        return totalPrice - operand;
                    case "Multiply":
                        return totalPrice * operand;
                    case "Divide":
                        return totalPrice / operand;
                    case "":
                        return totalPrice;
                    default:
                        throw new ArgumentException("Invalid operator");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<int> CreateTranslationGroupAsync()
        {
            try
            {
                return await _quoteQueryRepo.CreateTranslationGroupAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<List<WaterMeterViewDto>> GetWaterMeterRecordsViewAsync(int page, int pageSize)
        {
            try
            {
                var entities = await _quoteQueryRepo.GetWaterMeterRecordsViewAsync(page, pageSize);
                List<WaterMeterViewDto> dtos = new();

                foreach (var entity in entities)
                {
                    dtos.Add(WaterMeterMapper.ToViewDto(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<List<SealingPlateViewDto>> GetSealingPlateRecordsViewAsync(int page, int pageSize)
        {
            try
            {
                var entities = await _quoteQueryRepo.GetSealingPlateRecordsViewAsync(page, pageSize);
                List<SealingPlateViewDto> dtos = new();

                foreach (var entity in entities)
                {
                    dtos.Add(SealingPlateMapper.ToViewDto(entity));
                }
                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }



        public async Task<int> GetTotalRecordCountAsync<TDto>() where TDto : class
        {
            if (!DtoEntityMapper.DtoToEntityMap.TryGetValue(typeof(TDto), out var entityType))
                throw new InvalidOperationException($"沒有對應的 entity type for {typeof(TDto).Name}");

            var method = _quoteQueryRepo.GetType()
                .GetMethod(nameof(IQuoteQueryRepo.GetTotalRecordCountAsync), BindingFlags.Public | BindingFlags.Instance)?
                .MakeGenericMethod(entityType)
                ?? throw new InvalidOperationException("找不到泛型 GetRecordTotalCountAsync");

            var task = (Task)method.Invoke(_quoteQueryRepo, new object[] { })!;
            await task.ConfigureAwait(false);

            // 取得 Task<int> 的 Result
            var resultProperty = task.GetType().GetProperty("Result");
            if (resultProperty == null)
                throw new InvalidOperationException("Task 沒有 Result 屬性");
            var result = resultProperty.GetValue(task);
            if (result is not int intResult)
                throw new InvalidOperationException("Result 不是 int 型別");
            return intResult;
        }

        public async Task<WaterMeterEditDto> GetWaterMeterRecordByIdAsync(int id)
        {
            var entity = await _quoteQueryRepo.GetWaterMeterRecordAsync(id);
            if (entity == null) throw new InvalidOperationException("Record not found");
            return WaterMeterMapper.ToEditDto(entity);
        }

        public async Task<SealingPlateEditDto> GetSealingPlateRecordByIdAsync(int id)
        {
            var entity = await _quoteQueryRepo.GetRecordByIdAsync<SealingPlateRecord>(id);
            if (entity == null) throw new InvalidOperationException("Record not found");
            return SealingPlateMapper.ToEditDto(entity);
        }
    }
}
