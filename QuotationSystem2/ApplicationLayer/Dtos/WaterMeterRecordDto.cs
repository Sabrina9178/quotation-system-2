using CommunityToolkit.Mvvm.ComponentModel;
using FirstFloor.ModernUI.Presentation;
using QuotationSystem2.ApplicationLayer.DTOs;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Models;
using QuotationSystem2.PresentationLayer.Views;
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

        public decimal PipeUnitPrice { get; set; } //added

        public decimal PipeLength { get; set; }

        public decimal PipePrice { get; set; }

        public int PipeTopID { get; set; }

        public decimal? PipeTopUnitPrice { get; set; } //added

        public int? PipeTopQuantity { get; set; }

        public decimal? PipeTopPrice { get; set; }

        public bool EndPlate { get; set; }

        public decimal? EndPlatePrice { get; set; }

        public int CustomerID { get; set; }

        public decimal CustomerDiscount { get; set; } // added

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

    public partial class WaterMeterViewDto : ObservableObject
    {
        public int WaterMeterID { get; set; }
        public DateTime Date { get; set; }
        public string PipeDiameter { get; set; } = null!;
        public string PipeThickness { get; set; } = null!;
        public decimal PipeLength { get; set; }
        public decimal PipeUnitPrice { get; set; }
        public decimal PipePrice { get; set; }
        public List<TranslationDto> PipeTopDisplayNames { get; set; } = null!;
        public decimal? PipeTopUnitPrice { get; set; }
        public int? PipeTopQuantity { get; set; }
        public decimal? PipeTopPrice { get; set; }
        public int TeethTypeCount { get; set; }
        public bool EndPlate { get; set; }
        public decimal? EndPlatePrice { get; set; }
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

        [ObservableProperty] private bool isSelected = false;
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
                PipeUnitPrice = entity.PipeUnitPrice,
                PipeLength = entity.PipeLength,
                PipePrice = entity.PipePrice,
                PipeTopID = entity.PipeTopID,
                PipeTopQuantity = entity.PipeTopQuantity,
                PipeTopPrice = entity.PipeTopPrice,
                PipeTopUnitPrice = entity.PipeTopUnitPrice,
                EndPlate = entity.EndPlate,
                EndPlatePrice = entity.EndPlatePrice,
                CustomerID = entity.CustomerID,
                CustomerDiscount = entity.CustomerDiscount,
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

        public static WaterMeterRecord ToEntity(WaterMeterEditDto dto, bool isCreate)
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
                PipeUnitPrice = dto.PipeUnitPrice,
                PipeLength = dto.PipeLength,
                PipePrice = dto.PipePrice,
                PipeTopID = dto.PipeTopID,
                PipeTopUnitPrice = dto.PipeTopUnitPrice,
                PipeTopQuantity = dto.PipeTopQuantity,
                PipeTopPrice = dto.PipeTopPrice,
                EndPlate = dto.EndPlate,
                EndPlatePrice = dto.EndPlatePrice,
                CustomerID = dto.CustomerID,
                CustomerDiscount = dto.CustomerDiscount,
                TotalPriceBeforeDiscount = dto.TotalPriceBeforeDiscount,
                TotalPrice = dto.TotalPrice,
                Note = dto.Note,
                Operator = dto.Operator,
                Operand = dto.Operand,
                FinalPrice = dto.FinalPrice,
                EmployeeID = dto.EmployeeID,
                WaterMeterRecordNipples = dto.WaterMeterRecordNipples
                    .Select(nippleDto => WaterMeterNippleMapper.ToEntity(nippleDto, isCreate))
                    .ToList(),
                WaterMeterRecordSockets = dto.WaterMeterRecordSockets
                    .Select(socketDto => WaterMeterSocketMapper.ToEntity(socketDto, isCreate))
                    .ToList(),
            };

            return entity;
        }

        public static WaterMeterViewDto ToViewDto(WaterMeterRecord entity)
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
                PipeUnitPrice = entity.PipeUnitPrice,
                PipePrice = entity.PipePrice,
                PipeTopDisplayNames = entity.PipeTop.Translation.Translations
                    .Select(t => TranslationMapper.ToDto(t))
                    .ToList(),
                PipeTopUnitPrice = entity.PipeTopUnitPrice,
                PipeTopQuantity = entity.PipeTopQuantity,
                PipeTopPrice = entity.PipeTopPrice,
                TeethTypeCount = entity.WaterMeterRecordSockets.Count + entity.WaterMeterRecordNipples.Count,
                EndPlate = entity.EndPlate,
                EndPlatePrice = entity.EndPlatePrice,
                Customer = entity.PipeTop.Translation.Translations
                    .Select(t => TranslationMapper.ToDto(t))
                    .ToList(),
                CustomerDiscount = entity.CustomerDiscount,
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
                        return WaterMeterNippleMapper.ToViewDto(nipple);
                    })
                    .ToList(),
                WaterMeterRecordSockets = entity.WaterMeterRecordSockets
                    .Select(socket =>
                    {
                        return WaterMeterSocketMapper.ToViewDto(socket);
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

    //public class PreparedWaterMeterViewData
    //{
    //    public int WaterMeterID { get; set; }
    //    public decimal PipeUnitPrice { get; set; }
    //    public decimal PipeDiscount { get; set; }
    //    public decimal PipeTopUnitPrice { get; set; }
    //    public decimal CustomerDiscount { get; set; }

    //    public List<PreparedTeethViewData> SocketDatas { get; set; } = new();
    //    public List<PreparedTeethViewData> NippleDatas { get; set; } = new();
    //}

    #endregion


    public partial class WaterMeterTeethEditDto
    {
        public int TeethID { get; set; }
        public int WaterMeterID { get; set; }
        public int TeethDiameterID { get; set; }
        public decimal UnitPrice { get; set; }
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

    //public class PreparedTeethViewData
    //{
    //    public int TeethID { get; set; }
    //    public decimal UnitPrice { get; set; }
    //}


    #region WaterMeterNipple DTOs
    public partial class WaterMeterNippleEditDto : WaterMeterTeethEditDto
    {

    }

    public partial class WaterMeterNippleViewDto : WaterMeterTeethViewDto
    {

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
                UnitPrice = entity.UnitPrice,
                Quantity = entity.Quantity,
                Price = entity.Price
            };
        }

        public static WaterMeterRecordNipple ToEntity(WaterMeterNippleEditDto dto, bool isCreate)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (isCreate && (dto.TeethID != 0 || dto.WaterMeterID != 0))
                throw new ArgumentException("TeethID and WaterMeterID should not be set for new entities.");

            var entity = new WaterMeterRecordNipple
            {
                NippleDiameterID = dto.TeethDiameterID,
                Quantity = dto.Quantity,
                Price = dto.Price,
                UnitPrice = dto.UnitPrice,
                NippleID = isCreate ? 0 : dto.TeethID,
                WaterMeterID = isCreate ? 0 : dto.WaterMeterID
            };
            return entity;
        }

        public static WaterMeterNippleViewDto ToViewDto(WaterMeterRecordNipple entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new WaterMeterNippleViewDto
            {
                TeethType = "Nipple",
                TeethID = entity.NippleID,
                WaterMeterID = entity.WaterMeterID,
                TeethDiameter = entity.NippleDiameter.DisplayName,
                Quantity = entity.Quantity,
                UnitPrice = entity.UnitPrice,
                Price = entity.Price
            };
        }
    }

    
    #endregion


    #region WaterMeterSocket DTOs
    public partial class WaterMeterSocketEditDto : WaterMeterTeethEditDto
    {

    }

    public partial class WaterMeterSocketViewDto : WaterMeterTeethViewDto
    {

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
                UnitPrice = entity.UnitPrice,
                Quantity = entity.Quantity,
                Price = entity.Price
            };
        }

        public static WaterMeterRecordSocket ToEntity(WaterMeterSocketEditDto dto, bool isCreate)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (isCreate && (dto.TeethID != 0 || dto.WaterMeterID != 0))
                throw new ArgumentException("SocketID and WaterMeterID should not be set for new entities.");

            var entity = new WaterMeterRecordSocket
            {
                SocketDiameterID = dto.TeethDiameterID,
                Quantity = dto.Quantity,
                Price = dto.Price,
                UnitPrice = dto.UnitPrice,
                SocketID = isCreate ? 0 : dto.TeethID,
                WaterMeterID = isCreate ? 0 : dto.WaterMeterID
            };
            return entity;
        }

        public static WaterMeterSocketViewDto ToViewDto(WaterMeterRecordSocket entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new WaterMeterSocketViewDto
            {
                TeethType = "Socket",
                TeethID = entity.SocketID,
                WaterMeterID = entity.WaterMeterID,
                TeethDiameter = entity.SocketDiameter.DisplayName,
                Quantity = entity.Quantity,
                UnitPrice = entity.UnitPrice,
                Price = entity.Price
            };
        }
    }
    #endregion
}

