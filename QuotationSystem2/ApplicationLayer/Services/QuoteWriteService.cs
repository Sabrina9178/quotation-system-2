using QuotationSystem2.ApplicationLayer.Common;
using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.ApplicationLayer.DTOs;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Models;
using QuotationSystem2.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuotationSystem2.ApplicationLayer.Services
{
    internal class QuoteWriteService : IQuoteWriteService
    {
        private readonly IQuoteWriteRepo _quoteWriteRepo;

        public QuoteWriteService(IQuoteWriteRepo quoteWriteRepo)
        {
            _quoteWriteRepo = quoteWriteRepo;
        }

        public async Task AddWaterMeterRecordAsync(WaterMeterEditDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Record cannot be null");
            }
            var entity = WaterMeterMapper.ToEntity(dto, true); // isCreate: true
            await _quoteWriteRepo.AddRecord<WaterMeterRecord>(entity);
        }

        public async Task AddSealingPlateRecordAsync(SealingPlateEditDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Record cannot be null");
            }
            var entity = SealingPlateMapper.ToEntity(dto, true); // isCreate: true
            await _quoteWriteRepo.AddRecord<SealingPlateRecord>(entity);
        }

        public async Task EditWaterMeterRecordAsync(WaterMeterEditDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Record cannot be null");
            }
            var entity = WaterMeterMapper.ToEntity(dto, false); // isCreate: false
            await _quoteWriteRepo.EditWaterMeterRecordAsync(entity);
        }
        public async Task EditSealingPlateRecordAsync(SealingPlateEditDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Record cannot be null");
            }
            var entity = SealingPlateMapper.ToEntity(dto, false); // isCreate: false
            await _quoteWriteRepo.EditSealingPlateRecordAsync(entity);
        }

        public async Task DeleteAllRecordsAsync<TDto>() where TDto : class
        {
            if (!DtoEntityMapper.DtoToEntityMap.TryGetValue(typeof(TDto), out var entityType))
                throw new InvalidOperationException($"沒有對應的 entity type for {typeof(TDto).Name}");

            var method = _quoteWriteRepo.GetType()
                .GetMethod(nameof(IQuoteWriteRepo.DeleteAllRecordsAsync), BindingFlags.Public | BindingFlags.Instance)?
                .MakeGenericMethod(entityType)
                ?? throw new InvalidOperationException("找不到泛型 DeleteAllRecordAsync");

            await (Task)method.Invoke(_quoteWriteRepo, null)!;
        }

        public async Task DeleteRecordsByIDAsync<TDto>(List<int> ids) where TDto : class
        {
            if (!DtoEntityMapper.DtoToEntityMap.TryGetValue(typeof(TDto), out var entityType))
                throw new InvalidOperationException($"沒有對應的 entity type for {typeof(TDto).Name}");

            var method = _quoteWriteRepo.GetType()
                .GetMethod(nameof(IQuoteWriteRepo.DeleteRecordsByIDAsync), BindingFlags.Public | BindingFlags.Instance)?
                .MakeGenericMethod(entityType)
                ?? throw new InvalidOperationException("找不到泛型 DeleteRecordsByIDAsync");

            await (Task)method.Invoke(_quoteWriteRepo, new object[] { ids })!;
        }

    }
}
