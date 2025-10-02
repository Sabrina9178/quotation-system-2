using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotationSystem2.DataAccessLayer.Models;

namespace QuotationSystem2.DataAccessLayer.Interfaces
{
    internal interface IQuoteWriteRepo
    {
        public Task AddRecord<T>(T record) where T : class;

        public Task EditRecord<T>(T record) where T : class;

        public Task EditWaterMeterRecordAsync(WaterMeterRecord record);

        public Task EditSealingPlateRecordAsync(SealingPlateRecord record);   

        public Task DeleteAllRecordsAsync<T>() where T : class;

        public Task DeleteRecordsByIDAsync<T>(List<int> ids) where T : class;
    }
}
