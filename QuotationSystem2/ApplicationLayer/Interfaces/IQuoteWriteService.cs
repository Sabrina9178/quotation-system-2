using QuotationSystem2.ApplicationLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.ApplicationLayer.Interfaces
{
    internal interface IQuoteWriteService
    {
        public Task AddWaterMeterRecordAsync(WaterMeterEditDto dto);

        public Task AddSealingPlateRecordAsync(SealingPlateEditDto dto);

        public Task EditWaterMeterRecordAsync(WaterMeterEditDto dto);

        public Task EditSealingPlateRecordAsync(SealingPlateEditDto dto);

        public Task DeleteAllRecordsAsync<T>() where T : class;

        public Task DeleteRecordsByIDAsync<TDto>(List<int> ids) where TDto : class;
    }
}
