using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.ApplicationLayer.Common
{
    public class DtoEntityMapper
    {
        public static readonly Dictionary<Type, Type> DtoToEntityMap = new()
        {
            { typeof(WaterMeterEditDto), typeof(WaterMeterRecord) },
            { typeof(WaterMeterViewDto), typeof(WaterMeterRecord) },
            { typeof(SealingPlateEditDto), typeof(SealingPlateRecord) },
            { typeof(SealingPlateViewDto), typeof(SealingPlateRecord) },
            //{ typeof(SheerStudEditDto), typeof(SheerStudRecord) },
            //{ typeof(SheerStudViewDto), typeof(SheerStudRecord) },
        };
    }
}
