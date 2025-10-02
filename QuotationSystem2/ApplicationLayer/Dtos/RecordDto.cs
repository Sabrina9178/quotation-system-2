using FirstFloor.ModernUI.Presentation;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Models;
using QuotationSystem2.PresentationLayer.Views;

//using QuotationSystem2.DataAccessLayer.Models.RecordModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuotationSystem2.ApplicationLayer.Dtos
{
    #region WaterMeterRecord DTOs
    public class WaterMeterEditDto
    {
        public int WaterMeterID { get; set; }

        public DateTime Date { get; set; }

        public int PipeDiameterID { get; set; }

        public int PipeThicknessID { get; set; }

        public decimal PipeLength { get; set; }

        public decimal PipePrice { get; set; }

        public int PipeTopID { get; set; }

        public int? PipeTopQuantity { get; set; }

        public decimal? PipeTopPrice { get; set; }

        public bool EndPlate { get; set; }

        public decimal? EndPlatePrice { get; set; }

        public int CustomerID { get; set; }

        public decimal TotalPriceBeforeDiscount { get; set; }

        public decimal TotalPrice { get; set; }

        public string? Note { get; set; }

        public int? Operator { get; set; }

        public decimal? Operand { get; set; }

        public decimal FinalPrice { get; set; }

        public int EmployeeID { get; set; }


        public List<WaterMeterNippleEditDto> WaterMeterRecordNipples { get; set; } = new();

        public List<WaterMeterSocketEditDto> WaterMeterRecordSockets { get; set; } = new();
    }

    public class WaterMeterViewDto
    {
        public int WaterMeterID { get; set; }
        public DateTime Date { get; set; }
        public string PipeDiameter { get; set; } = null!;
        public string PipeThickness { get; set; } = null!;
        public decimal PipeLength { get; set; }
        public decimal PipeUnitPrice { get; set; }
        public decimal PipeDiscount { get; set; } // fix: can delete
        public decimal PipePrice { get; set; }
        public List<TranslationDto> PipeTopDisplayNames { get; set; } = null!;
        public decimal PipeTopUnitPrice { get; set; }
        public int? PipeTopQuantity { get; set; }
        public decimal? PipeTopPrice { get; set; }
        public int TeethTypeCount { get; set; }
        public bool EndPlate { get; set; }
        public decimal EndPlatePrice { get; set; }
        public List<TranslationDto> Customer { get; set; } = null!;
        public decimal CustomerDiscount { get; set; }
        public decimal TotalPriceBeforeDiscount { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
        public string? Operator { get; set; }
        public decimal? Operand { get; set; }
        public decimal FinalPrice { get; set; }
        public string Employee { get; set; } = null!;
        public List<WaterMeterNippleViewDto> WaterMeterRecordNipples { get; set; } = new();
        public List<WaterMeterSocketViewDto> WaterMeterRecordSockets { get; set; } = new();

        // Command to open the popup
        public RelayCommand OpenTeethPopupCommand { get; set; } = null!;
    }

    public static class WaterMeterMapper
    {
        public static WaterMeterEditDto ToEditDto(WaterMeterRecord entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new WaterMeterEditDto
            {
                WaterMeterID = entity.WaterMeterID,
                Date = entity.Date,
                PipeDiameterID = entity.PipeDiameterID,
                PipeThicknessID = entity.PipeThicknessID,
                PipeLength = entity.PipeLength,
                PipePrice = entity.PipePrice,
                PipeTopID = entity.PipeTopID,
                PipeTopQuantity = entity.PipeTopQuantity,
                PipeTopPrice = entity.PipeTopPrice,
                EndPlate = entity.EndPlate,
                EndPlatePrice = entity.EndPlatePrice,
                CustomerID = entity.CustomerID,
                TotalPriceBeforeDiscount = entity.TotalPriceBeforeDiscount,
                TotalPrice = entity.TotalPrice,
                Note = entity.Note,
                Operator = entity.Operator,
                Operand = entity.Operand,
                FinalPrice = entity.FinalPrice,
                EmployeeID = entity.EmployeeID,
                WaterMeterRecordNipples = entity.WaterMeterRecordNipples.Select(WaterMeterNippleMapper.ToEditDto).ToList(),
                WaterMeterRecordSockets = entity.WaterMeterRecordSockets.Select(WaterMeterSocketMapper.ToEditDto).ToList()
            };
        }

        public static WaterMeterRecord ToEntity(WaterMeterEditDto dto, bool isCreate = false)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (isCreate && (dto.WaterMeterID != 0))
                throw new ArgumentException("WaterMeterID should not be set for new entities.");

            var entity = new WaterMeterRecord
            {
                WaterMeterID = isCreate ? 0 : dto.WaterMeterID,
                Date = dto.Date,
                PipeDiameterID = dto.PipeDiameterID,
                PipeThicknessID = dto.PipeThicknessID,
                PipeLength = dto.PipeLength,
                PipePrice = dto.PipePrice,
                PipeTopID = dto.PipeTopID,
                PipeTopQuantity = dto.PipeTopQuantity,
                PipeTopPrice = dto.PipeTopPrice,
                EndPlate = dto.EndPlate,
                EndPlatePrice = dto.EndPlatePrice,
                CustomerID = dto.CustomerID,
                TotalPriceBeforeDiscount = dto.TotalPriceBeforeDiscount,
                TotalPrice = dto.TotalPrice,
                Note = dto.Note,
                Operator = dto.Operator,
                Operand = dto.Operand,
                FinalPrice = dto.FinalPrice,
                EmployeeID = dto.EmployeeID,
                WaterMeterRecordNipples = dto.WaterMeterRecordNipples
                    .Select(nippleDto => WaterMeterNippleMapper.ToEntity(nippleDto))
                    .ToList(),
                WaterMeterRecordSockets = dto.WaterMeterRecordSockets
                    .Select(socketDto => WaterMeterSocketMapper.ToEntity(socketDto))
                    .ToList(),
            };

            return entity;
        }

        public static WaterMeterViewDto ToViewDto(WaterMeterRecord entity, PreparedWaterMeterViewData preparedData)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var pipeTopDisplayNames = entity.PipeTop.Translation.Translations
            .Select(t => TranslationMapper.ToDto(t))
            .ToList();


            var dto = new WaterMeterViewDto
            {
                WaterMeterID = entity.WaterMeterID,
                Date = entity.Date,
                PipeDiameter = entity.PipeDiameter.DisplayName,
                PipeThickness = entity.PipeThickness.DisplayName,
                PipeLength = entity.PipeLength,
                PipeUnitPrice = preparedData.PipeUnitPrice,
                PipeDiscount = preparedData.PipeDiscount,
                PipePrice = entity.PipePrice,
                PipeTopDisplayNames = entity.PipeTop.Translation.Translations
                    .Select(t => TranslationMapper.ToDto(t))
                    .ToList(),
                PipeTopUnitPrice = preparedData.PipeTopUnitPrice,
                PipeTopQuantity = entity.PipeTopQuantity,
                PipeTopPrice = entity.PipeTopPrice,
                TeethTypeCount = entity.WaterMeterRecordSockets.Count + entity.WaterMeterRecordNipples.Count,
                EndPlate = entity.EndPlate,
                EndPlatePrice = entity.EndPlatePrice ?? 0, //fix EndPlatePrice分明可以為null
                Customer = entity.PipeTop.Translation.Translations
                    .Select(t => TranslationMapper.ToDto(t))
                    .ToList(),
                CustomerDiscount = preparedData.CustomerDiscount,
                TotalPriceBeforeDiscount = entity.TotalPriceBeforeDiscount,
                TotalPrice = entity.TotalPrice,
                Note = entity.Note,
                Operator = entity.OperatorNavigation?.DisplayName ?? "None",
                Operand = entity.Operand,
                FinalPrice = entity.FinalPrice,
                Employee = entity.Employee.Name,

                WaterMeterRecordNipples = entity.WaterMeterRecordNipples
                    .Select(nipple =>
                    {
                        // 根據 NippleID 或 NippleDiameterID 找到對應的 preparedData
                        var prepared = preparedData.NippleDatas
                            .FirstOrDefault(pd => pd.TeethID == nipple.NippleDiameterID);
                        // 若找不到則給預設值
                        if (prepared == null)
                            prepared = new PreparedTeethViewData { TeethID = nipple.NippleDiameterID, UnitPrice = 0 };
                        return WaterMeterNippleMapper.ToViewDto(nipple, prepared);
                    })
                    .ToList(),
                WaterMeterRecordSockets = entity.WaterMeterRecordSockets
                    .Select(socket =>
                    {
                        // 根據 SocketID 或 SocketDiameterID 找到對應的 preparedData
                        var prepared = preparedData.SocketDatas
                            .FirstOrDefault(pd => pd.TeethID == socket.SocketDiameterID);
                        // 若找不到則給預設值
                        if (prepared == null)
                            prepared = new PreparedTeethViewData { TeethID = socket.SocketDiameterID, UnitPrice = 0 };
                        return WaterMeterSocketMapper.ToViewDto(socket, prepared);
                    })
                    .ToList()
            };

            dto.OpenTeethPopupCommand = new RelayCommand(param => OpenTeethPopup(dto.WaterMeterRecordSockets, dto.WaterMeterRecordNipples));

            return dto;
        }

        private static void OpenTeethPopup(List<WaterMeterSocketViewDto> socketDtos, List<WaterMeterNippleViewDto> nippleDtos)
        {
            if (socketDtos != null && nippleDtos != null)
            {
                var dtos = new List<WaterMeterTeethViewDto>();
                // Combine socket and nipple DTOs
                dtos.AddRange(socketDtos);
                dtos.AddRange(nippleDtos);

                // change dtos to ObservableCollection
                var observableCollection = new ObservableCollection<WaterMeterTeethViewDto>(dtos);

                // Open the popup and pass related Teeth data
                WaterMeterTeethPopup popup = new WaterMeterTeethPopup(observableCollection);
                popup.ShowDialog();
            }
            else
            {
                System.Windows.MessageBox.Show("沒有內牙或外牙");
                return;
            }
        }
    }

    

    public class PreparedWaterMeterViewData
    {
        public int WaterMeterID { get; set; }
        public decimal PipeUnitPrice { get; set; }
        public decimal PipeDiscount { get; set; }
        public decimal PipeTopUnitPrice { get; set; }
        public decimal CustomerDiscount { get; set; }

        public List<PreparedTeethViewData> SocketDatas { get; set; } = new();
        public List<PreparedTeethViewData> NippleDatas { get; set; } = new();
    }

    #endregion

    //#region WaterMeterTeeth DTOs
    //public partial class WaterMeterTeethEditDto
    //{
    //    public string TeethType { get; set; } = null!;

    //    public int TeethID { get; set; }

    //    public int WaterMeterID { get; set; }

    //    public int TeethDiameterID { get; set; }

    //    public int Quantity { get; set; }

    //    public decimal Price { get; set; }
    //}

    //public partial class WaterMeterTeethViewDto
    //{
    //    public string TeethType { get; set; } = null!;
    //    public int TeethID { get; set; }
    //    public int WaterMeterID { get; set; }
    //    public string TeethDiameter { get; set; } = null!;
    //    public int Quantity { get; set; }
    //    public decimal UnitPrice { get; set; }
    //    public decimal Price { get; set; }
    //}

    //public static class WaterMeterTeethMapper
    //{
    //    public static WaterMeterTeethEditDto ToEditDtoNipple(WaterMeterRecordNipple entity)
    //    {
    //        if (entity == null) throw new ArgumentNullException(nameof(entity));
    //        return new WaterMeterTeethEditDto
    //        {
    //            TeethType = "Nipple",
    //            TeethID = entity.NippleID,
    //            WaterMeterID = entity.WaterMeterID,
    //            TeethDiameterID = entity.NippleDiameterID,
    //            Quantity = entity.Quantity,
    //            Price = entity.Price
    //        };
    //    }

    //    public static WaterMeterTeethEditDto ToEditDtoSocket(WaterMeterRecordSocket entity)
    //    {
    //        if (entity == null) throw new ArgumentNullException(nameof(entity));
    //        return new WaterMeterTeethEditDto
    //        {
    //            TeethType = "Socket",
    //            TeethID = entity.SocketID,
    //            WaterMeterID = entity.WaterMeterID,
    //            TeethDiameterID = entity.SocketDiameterID,
    //            Quantity = entity.Quantity,
    //            Price = entity.Price
    //        };
    //    }

    //    public static WaterMeterRecordNipple ToEntityNipple(WaterMeterTeethEditDto dto, bool isCreate = false)
    //    {
    //        if (dto == null) throw new ArgumentNullException(nameof(dto));
    //        if (isCreate && (dto.TeethID != 0 || dto.WaterMeterID != 0))
    //            throw new ArgumentException("TeethID and WaterMeterID should not be set for new entities.");

    //        var entity = new WaterMeterRecordNipple
    //        {
    //            NippleDiameterID = dto.TeethDiameterID,
    //            Quantity = dto.Quantity,
    //            Price = dto.Price,
    //            NippleID = isCreate ? 0 : dto.TeethID,
    //            WaterMeterID = isCreate ? 0 : dto.WaterMeterID
    //        };
    //        return entity;
    //    }

    //    public static WaterMeterRecordSocket ToEntitySocket(WaterMeterTeethEditDto dto, bool isCreate = false)
    //    {
    //        if (dto == null) throw new ArgumentNullException(nameof(dto));
    //        if (isCreate && (dto.TeethID != 0 || dto.WaterMeterID != 0))
    //            throw new ArgumentException("TeethID and WaterMeterID should not be set for new entities.");

    //        var entity = new WaterMeterRecordSocket
    //        {
    //            SocketDiameterID = dto.TeethDiameterID,
    //            Quantity = dto.Quantity,
    //            Price = dto.Price,
    //            SocketID = isCreate ? 0 : dto.TeethID,
    //            WaterMeterID = isCreate ? 0 : dto.WaterMeterID
    //        };
    //        return entity;
    //    }

    //    public static WaterMeterNippleViewDto ToViewDto(WaterMeterRecordNipple entity, PreparedTeethViewData preparedData)
    //    {
    //        if (entity == null) throw new ArgumentNullException(nameof(entity));
    //        return new WaterMeterNippleViewDto
    //        {
    //            NippleID = entity.NippleID,
    //            WaterMeterID = entity.WaterMeterID,
    //            NippleDiameter = entity.NippleDiameter.DisplayName,
    //            Quantity = entity.Quantity,
    //            UnitPrice = preparedData.UnitPrice,
    //            Price = entity.Price
    //        };
    //    }
    //}

    //public class PreparedTeethViewData
    //{
    //    public int TeethID { get; set; }
    //    public decimal UnitPrice { get; set; }
    //}
    //#endregion


    public partial class WaterMeterTeethEditDto
    {
        public int TeethID { get; set; }

        public int WaterMeterID { get; set; }

        public int TeethDiameterID { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }

    public partial class WaterMeterTeethViewDto
    {
        public string TeethType { get; set; } = null!;
        public int TeethID { get; set; }
        public int WaterMeterID { get; set; }
        public string TeethDiameter { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
    }



    #region WaterMeterNipple DTOs
    public partial class WaterMeterNippleEditDto : WaterMeterTeethEditDto
    {
        //public int NippleID { get; set; }

        //public int WaterMeterID { get; set; }

        //public int NippleDiameterID { get; set; }

        //public int Quantity { get; set; }

        //public decimal Price { get; set; }
    }

    public partial class WaterMeterNippleViewDto : WaterMeterTeethViewDto
    {
        //public int NippleID { get; set; }
        //public int WaterMeterID { get; set; }
        //public string NippleDiameter { get; set; } = null!;
        //public int Quantity { get; set; }
        //public decimal UnitPrice { get; set; }
        //public decimal Price { get; set; }
    }

    public static class WaterMeterNippleMapper
    {
        public static WaterMeterNippleEditDto ToEditDto(WaterMeterRecordNipple entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new WaterMeterNippleEditDto
            {
                TeethID = entity.NippleID,
                WaterMeterID = entity.WaterMeterID,
                TeethDiameterID = entity.NippleDiameterID,
                Quantity = entity.Quantity,
                Price = entity.Price
            };
        }

        public static WaterMeterRecordNipple ToEntity(WaterMeterNippleEditDto dto, bool isCreate = false)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (isCreate && (dto.TeethID != 0 || dto.WaterMeterID != 0))
                throw new ArgumentException("TeethID and WaterMeterID should not be set for new entities.");

            var entity = new WaterMeterRecordNipple
            {
                NippleDiameterID = dto.TeethDiameterID,
                Quantity = dto.Quantity,
                Price = dto.Price,
                NippleID = isCreate ? 0 : dto.TeethID,
                WaterMeterID = isCreate ? 0 : dto.WaterMeterID
            };
            return entity;
        }

        public static WaterMeterNippleViewDto ToViewDto(WaterMeterRecordNipple entity, PreparedTeethViewData preparedData)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new WaterMeterNippleViewDto
            {
                TeethType = "Nipple",
                TeethID = entity.NippleID,
                WaterMeterID = entity.WaterMeterID,
                TeethDiameter = entity.NippleDiameter.DisplayName,
                Quantity = entity.Quantity,
                UnitPrice = preparedData.UnitPrice,
                Price = entity.Price
            };
        }
    }

    public class PreparedTeethViewData
    {
        public int TeethID { get; set; }
        public decimal UnitPrice { get; set; }
    }
    #endregion


    #region WaterMeterSocket DTOs
    public partial class WaterMeterSocketEditDto : WaterMeterTeethEditDto
    {
        //public int SocketID { get; set; }
        //public int WaterMeterID { get; set; }
        //public int SocketDiameterID { get; set; }
        //public int Quantity { get; set; }
        //public decimal Price { get; set; }
    }
    public partial class WaterMeterSocketViewDto : WaterMeterTeethViewDto
    {
        //public int SocketID { get; set; }
        //public int WaterMeterID { get; set; }
        //public string SocketDiameter { get; set; } = null!;
        //public int Quantity { get; set; }
        //public decimal UnitPrice { get; set; }
        //public decimal Price { get; set; }
    }

    public static class WaterMeterSocketMapper
    {
        public static WaterMeterSocketEditDto ToEditDto(WaterMeterRecordSocket entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new WaterMeterSocketEditDto
            {
                TeethID = entity.SocketID,
                WaterMeterID = entity.WaterMeterID,
                TeethDiameterID = entity.SocketDiameterID,
                Quantity = entity.Quantity,
                Price = entity.Price
            };
        }

        public static WaterMeterRecordSocket ToEntity(WaterMeterSocketEditDto dto, bool isCreate = false)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (isCreate && (dto.TeethID != 0 || dto.WaterMeterID != 0))
                throw new ArgumentException("SocketID and WaterMeterID should not be set for new entities.");

            var entity = new WaterMeterRecordSocket
            {
                SocketDiameterID = dto.TeethDiameterID,
                Quantity = dto.Quantity,
                Price = dto.Price,
                SocketID = isCreate ? 0 : dto.TeethID,
                WaterMeterID = isCreate ? 0 : dto.WaterMeterID
            };
            return entity;
        }

        public static WaterMeterSocketViewDto ToViewDto(WaterMeterRecordSocket entity, PreparedTeethViewData preparedData)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new WaterMeterSocketViewDto
            {
                TeethType = "Socket",
                TeethID = entity.SocketID,
                WaterMeterID = entity.WaterMeterID,
                TeethDiameter = entity.SocketDiameter.DisplayName,
                Quantity = entity.Quantity,
                UnitPrice = preparedData.UnitPrice,
                Price = entity.Price
            };
        }
    }
    #endregion
}

