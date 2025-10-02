using CommunityToolkit.Mvvm.ComponentModel;
using QuotationSystem2.PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotationSystem2.DataAccessLayer.Models;

namespace QuotationSystem2.ApplicationLayer.Dtos
{
    public class SealingPlateEditDto
    {
        public int SealingPlateID { get; set; }

        public DateTime Date { get; set; }

        public int ProductID { get; set; }

        public int PipeDiameterID { get; set; }

        public int PipeThicknessID { get; set; }

        public decimal PipeUnitPrice { get; set; } //added

        public decimal PipeDiscount { get; set; }

        public decimal PipeLength { get; set; }

        public decimal PipePrice { get; set; }

        public int PipeTopID { get; set; }

        public decimal? PipeTopUnitPrice { get; set; } //added

        public int? PipeTopQuantity { get; set; }

        public decimal? PipeTopPrice { get; set; }

        public int SocketDiameterID { get; set; }

        public decimal? SocketUnitPrice { get; set; }

        public decimal WeldingUnitPrice { get; set; }

        public int? SocketQuantity { get; set; }

        public int WeldingQuantity { get; set; }

        public decimal SocketPrice { get; set; }

        public decimal SealingPlatePrice { get; set; }

        public int CustomerID { get; set; }

        public decimal CustomerDiscount { get; set; } // added

        public decimal TotalPriceBeforeDiscount { get; set; }

        public decimal TotalPrice { get; set; }

        public string? Note { get; set; }

        public int? Operator { get; set; }

        public decimal? Operand { get; set; }

        public decimal FinalPrice { get; set; }

        public int EmployeeID { get; set; }
    }

    public partial class SealingPlateViewDto : ObservableObject
    {
        public int SealingPlateID { get; set; }
        public DateTime Date { get; set; }
        public string PipeDiameter { get; set; } = null!;
        public List<TranslationDto> ProductDisplayNames { get; set; } = null!;
        public decimal PipeDiscount { get; set; }
        public string PipeThickness { get; set; } = null!;
        public decimal PipeLength { get; set; }
        public decimal PipeUnitPrice { get; set; }
        public decimal PipePrice { get; set; }
        public List<TranslationDto> PipeTopDisplayNames { get; set; } = null!;
        public decimal? PipeTopUnitPrice { get; set; }
        public int? PipeTopQuantity { get; set; }
        public decimal? PipeTopPrice { get; set; }

        public string SocketDiameter { get; set; } = null!;
        public decimal? SocketUnitPrice { get; set; }
        public decimal WeldingUnitPrice { get; set; }
        public int? SocketQuantity { get; set; }
        public int WeldingQuantity { get; set; }
        public decimal SocketPrice { get; set; }
        public decimal SealingPlatePrice { get; set; }
        public List<TranslationDto> Customer { get; set; } = null!;
        public decimal CustomerDiscount { get; set; }
        public decimal TotalPriceBeforeDiscount { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
        public string? Operator { get; set; }
        public decimal? Operand { get; set; }
        public decimal FinalPrice { get; set; }
        public string Employee { get; set; } = null!;

        [ObservableProperty] private bool isSelected = false;
    }

    public static class SealingPlateMapper
    {
        public static SealingPlateEditDto ToEditDto(SealingPlateRecord entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new SealingPlateEditDto
            {
                SealingPlateID = entity.SealingPlateID,
                Date = entity.Date,
                ProductID = entity.ProductID,
                PipeDiameterID = entity.PipeDiameterID,
                PipeThicknessID = entity.PipeThicknessID,
                PipeUnitPrice = entity.PipeUnitPrice,
                PipeLength = entity.PipeLength,
                PipePrice = entity.PipePrice,
                PipeTopID = entity.PipeTopID,
                PipeTopUnitPrice = entity.PipeTopUnitPrice,
                PipeTopQuantity = entity.PipeTopQuantity,
                PipeTopPrice = entity.PipeTopPrice,
                SocketDiameterID = entity.SocketDiameterID,
                SocketUnitPrice = entity.SocketUnitPrice,
                WeldingUnitPrice = entity.WeldingUnitPrice,
                SocketQuantity = entity.SocketQuantity,
                WeldingQuantity = entity.WeldingQuantity,
                SocketPrice = entity.SocketPrice,
                SealingPlatePrice = entity.SealingPlatePrice,
                CustomerID = entity.CustomerID,
                CustomerDiscount = entity.CustomerDiscount,
                TotalPriceBeforeDiscount = entity.TotalPriceBeforeDiscount,
                TotalPrice = entity.TotalPrice,
                Note = entity.Note,
                Operator = entity.Operator,
                Operand = entity.Operand,
                FinalPrice = entity.FinalPrice,
                EmployeeID = entity.EmployeeID
            };
        }

        public static SealingPlateRecord ToEntity(SealingPlateEditDto dto, bool isCreate)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (isCreate && (dto.SealingPlateID != 0))
                throw new ArgumentException("SealingPlateID should not be set for new entities.");

            var entity = new SealingPlateRecord
            {
                SealingPlateID = isCreate ? 0 : dto.SealingPlateID,
                Date = dto.Date,
                ProductID = dto.ProductID,
                PipeDiameterID = dto.PipeDiameterID,
                PipeThicknessID = dto.PipeThicknessID,
                PipeUnitPrice = dto.PipeUnitPrice,
                PipeDiscount = dto.PipeDiscount,
                PipeLength = dto.PipeLength,
                PipePrice = dto.PipePrice,
                PipeTopID = dto.PipeTopID,
                PipeTopUnitPrice = dto.PipeTopUnitPrice,
                PipeTopQuantity = dto.PipeTopQuantity,
                PipeTopPrice = dto.PipeTopPrice,

                SocketDiameterID = dto.SocketDiameterID,
                SocketUnitPrice = dto.SocketUnitPrice,
                WeldingUnitPrice = dto.WeldingUnitPrice,
                SocketQuantity = dto.SocketQuantity,
                WeldingQuantity = dto.WeldingQuantity,
                SocketPrice = dto.SocketPrice,
                SealingPlatePrice = dto.SealingPlatePrice,

                CustomerID = dto.CustomerID,
                CustomerDiscount = dto.CustomerDiscount,
                TotalPriceBeforeDiscount = dto.TotalPriceBeforeDiscount,
                TotalPrice = dto.TotalPrice,
                Note = dto.Note,
                Operator = dto.Operator,
                Operand = dto.Operand,
                FinalPrice = dto.FinalPrice,
                EmployeeID = dto.EmployeeID,
            };

            return entity;
        }

        public static SealingPlateViewDto ToViewDto(SealingPlateRecord entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var pipeTopDisplayNames = entity.PipeTop.Translation.Translations
            .Select(t => TranslationMapper.ToDto(t))
            .ToList();

            var dto = new SealingPlateViewDto
            {
                SealingPlateID = entity.SealingPlateID,
                Date = entity.Date,
                ProductDisplayNames = entity.Product.Translation.Translations
                    .Select(t => TranslationMapper.ToDto(t))
                    .ToList(),
                PipeDiameter = entity.PipeDiameter.DisplayName,
                PipeThickness = entity.PipeThickness.DisplayName,
                PipeDiscount = entity.PipeDiscount,
                PipeLength = entity.PipeLength,
                PipeUnitPrice = entity.PipeUnitPrice,
                PipePrice = entity.PipePrice,
                PipeTopDisplayNames = entity.PipeTop.Translation.Translations
                    .Select(t => TranslationMapper.ToDto(t))
                    .ToList(),
                PipeTopUnitPrice = entity.PipeTopUnitPrice,
                PipeTopQuantity = entity.PipeTopQuantity,
                PipeTopPrice = entity.PipeTopPrice,

                SocketDiameter = entity.SocketDiameter.DisplayName,
                SocketUnitPrice = entity.SocketUnitPrice,
                WeldingUnitPrice = entity.WeldingUnitPrice,
                SocketQuantity = entity.SocketQuantity,
                WeldingQuantity = entity.WeldingQuantity,
                SocketPrice = entity.SocketPrice,
                SealingPlatePrice = entity.SealingPlatePrice,

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
            };

            return dto;
        }
    }
}
