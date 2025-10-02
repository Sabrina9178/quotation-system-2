using QuotationSystem2.ApplicationLayer.Dtos;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuotationSystem2.ApplicationLayer.Services
{
    internal class RecordService : IRecordService
    {
        //private List<PipePrice> pipeUnitPrices = new();
        //private List<PipeTopPrice> pipeTopUnitPrices = new();
        //private List<Customer> customers = new();
        //private List<Product> products = new();
        //private List<ComponentPrice> componentPrices = new();

        //private readonly IQuoteQueryRepo _quoteQueryRepo;

        //public RecordService(IQuoteQueryRepo quoteQueryRepo)
        //{
        //    _quoteQueryRepo = quoteQueryRepo;
        //    _ = InitializeAsync(); // fire-and-forget
        //}

        //public async Task InitializeAsync()
        //{
        //    pipeUnitPrices = await _quoteQueryRepo.GetPipePricesAsync();
        //    pipeTopUnitPrices = await _quoteQueryRepo.GetPipeTopPricesAsync();
        //    customers = await _quoteQueryRepo.GetCustomersAsync();
        //    products = await _quoteQueryRepo.GetProductsAsync();
        //    componentPrices = await _quoteQueryRepo.GetComponentPricesAsync();
        //}

        //public async Task<List<PreparedWaterMeterViewData>> WaterMeterViewDataPrepareAsync(IEnumerable<WaterMeterRecord> entities)
        //{
        //    List<PreparedWaterMeterViewData> preparedData = new();
        //    var pipeDiscount = this.products
        //        .Where(p => p.ProductName == "WaterMeter")
        //        .Select(p => p.PipeDiscount)
        //        .FirstOrDefault();
        //    int socketComponentID = await _quoteQueryRepo.GetComponentIDByNameAsync("SocketWaterMeter");
        //    int nippleComponentID = await _quoteQueryRepo.GetComponentIDByNameAsync("Nipple");
        //    foreach (var entity in entities)
        //    {
        //        preparedData.Add(new PreparedWaterMeterViewData
        //        {
        //            WaterMeterID = entity.WaterMeterID,
        //            PipeUnitPrice = this.pipeUnitPrices
        //                .FirstOrDefault(p => p.DiameterID == entity.PipeDiameterID && p.ThicknessID == entity.PipeThicknessID)?.PipeUnitPrice ?? 0,
        //            PipeDiscount = pipeDiscount,
        //            PipeTopUnitPrice = this.pipeTopUnitPrices
        //                .FirstOrDefault(p => p.DiameterID == entity.PipeDiameterID && p.PipeTopID == entity.PipeTopID)?.Price ?? 0,
        //            CustomerDiscount = this.customers
        //                .FirstOrDefault(c => c.CustomerID == entity.CustomerID)?.Discount ?? 0,
        //            SocketDatas = entity.WaterMeterRecordSockets.Select(socket => new PreparedTeethViewData
        //            {
        //                TeethID = socket.SocketID,
        //                UnitPrice = this.componentPrices
        //                    .FirstOrDefault(p => p.ComponentID == socketComponentID && p.DiameterID == socket.SocketDiameterID)?.Price ?? 0
        //            }).ToList(),
        //            NippleDatas = entity.WaterMeterRecordNipples.Select(nipple => new PreparedTeethViewData
        //            {
        //                TeethID = nipple.NippleID,
        //                UnitPrice = this.componentPrices
        //                    .FirstOrDefault(p => p.ComponentID == nippleComponentID && p.DiameterID == nipple.NippleDiameterID)?.Price ?? 0
        //            }).ToList()
        //        });
        //    }
        //    return preparedData;
        //}
    }
}
