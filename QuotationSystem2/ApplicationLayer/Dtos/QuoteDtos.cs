using CommunityToolkit.Mvvm.ComponentModel;
using QuotationSystem2.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuotationSystem2.ApplicationLayer.Dtos
{
    // Translation
    public partial class TranslationDto
    {
        public int TranslationID { get; set; }
        public string LanguageCode { get; set; } = null!;

        [StringLength(100)][Required]
        public string DisplayName { get; set; } = "";
    }
    public static class TranslationMapper
    {
        public static TranslationDto ToDto(Translation entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            // 用 LanguageMapping 反轉找 key (LanguageCode)
            var languageCode = LanguageMapping.FirstOrDefault(x => x.Value == entity.LanguageID).Key;
            if (languageCode == null)
                throw new ArgumentException("Unknown LanguageID");

            return new TranslationDto
            {
                TranslationID = entity.TranslationID,
                LanguageCode = languageCode,
                DisplayName = entity.DisplayName,
            };
        }

        public static Translation ToEntity(TranslationDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!LanguageMapping.TryGetValue(dto.LanguageCode, out var languageID))
                throw new ArgumentException("Unknown LanguageCode");

            return new Translation
            {
                TranslationID = dto.TranslationID,
                LanguageID = languageID,
                DisplayName = dto.DisplayName
            };
        }

        private static readonly Dictionary<string, int> LanguageMapping = new()
    {
        { "en-US", 1 },
        { "zh-TW", 2 }
    };
    }




    // PipeDiameter
    public partial class PipeDiameterDto
    {
        public int DiameterID { get; set; } // Nullable to allow for creation of new PipeDiameter without an ID

        [Range(0.01, 10000.00, ErrorMessage = "Diameter must be between 0.01 and 10000.00.")]
        public decimal Diameter { get; set; }

        [StringLength(50)][Required]
        public string DisplayName { get; set; } = null!;
    }
    public static class PipeDiameterMapper
    {
        public static PipeDiameterDto ToDto(PipeDiameter entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new PipeDiameterDto
            {
                DiameterID = entity.DiameterID,
                Diameter = entity.Diameter,
                DisplayName = entity.DisplayName
            };
        }

        public static PipeDiameter ToEntity(PipeDiameterDto dto, bool isCreate = false)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            if (isCreate && dto.DiameterID != 0)
                throw new ArgumentException("DiameterID should not be set for new PipeDiameter entities.");

            var entity = new PipeDiameter
            {
                Diameter = dto.Diameter,
                DisplayName = dto.DisplayName
            };

            if (!isCreate && dto.DiameterID != 0)
                entity.DiameterID = dto.DiameterID; // 用於更新時保留原始 ID

            return entity;
        }
    }

    // PipeThickness
    public partial class PipeThicknessDto
    {
        public int ThicknessID { get; set; }

        [StringLength(50)][Required]
        public string DisplayName { get; set; } = null!;
    }
    public static class PipeThicknessMapper
    {
        public static PipeThicknessDto ToDto(PipeThickness entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new PipeThicknessDto
            {
                ThicknessID = entity.ThicknessID,
                DisplayName = entity.DisplayName
            };
        }

        public static PipeThickness ToEntity(PipeThicknessDto dto, bool isCreate = false)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            if (isCreate && dto.ThicknessID != 0)
                throw new ArgumentException("ThicknessID should not be set for new PipeDiameter entities.");

            var entity = new PipeThickness
            {
                DisplayName = dto.DisplayName
            };

            if (!isCreate)
                entity.ThicknessID = dto.ThicknessID; // 用於更新時保留原始 ID

            return entity;
        }
    }

    // Product
    public partial class ProductDto
    {
        public int ProductID { get; set; }

        [StringLength(100)][Required]
        public string ProductName { get; set; } = null!;

        [Range(0.01, 10000.00, ErrorMessage = "Diameter must be between 0.01 and 10000.00.")]
        public decimal PipeDiscount { get; set; }

        public int TranslationID { get; set; }

        public List<TranslationDto> DisplayNames { get; set; } = new();
    }
    public static class ProductMapper
    {
        public static ProductDto ToDto(Product entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var dtoTranslations = entity.Translation.Translations
            .Select(TranslationMapper.ToDto)
            .ToList();

            return new ProductDto
            {
                ProductID = entity.ProductID,
                PipeDiscount = entity.PipeDiscount,
                ProductName = entity.ProductName,
                TranslationID = entity.TranslationID,
                DisplayNames = dtoTranslations
            };
        }
        public static Product ToEntity(ProductDto dto, bool isCreate = false)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (isCreate && dto.ProductID != 0)
                throw new ArgumentException("ProductID should not be set for new PipeDiameter entities.");

            var entity = new Product
            {
                ProductName = dto.ProductName,
                PipeDiscount = dto.PipeDiscount,
                TranslationID = dto.TranslationID,
                Translation = new TranslationGroup
                {
                    Translations = dto.DisplayNames.Select(TranslationMapper.ToEntity).ToList()
                }
            };

            if (!isCreate)
                entity.ProductID = dto.ProductID; // 用於更新時保留原始 ID

            return entity;
        }
    }

    // Component
    public partial class ComponentDto
    {
        public int ComponentID { get; set; }

        [StringLength(500)][Required]
        public string ComponentName { get; set; } = null!;
        public int TranslationID { get; set; }
        public List<TranslationDto> DisplayNames { get; set; } = new();
    }
    public static class ComponentMapper
    {
        public static ComponentDto ToDto(Component entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var dtoTranslations = entity.Translation.Translations
            .Select(TranslationMapper.ToDto)
            .ToList();

            return new ComponentDto
            {
                ComponentID = entity.ComponentID,
                TranslationID = entity.TranslationID,
                DisplayNames = dtoTranslations
            };
        }
        public static Component ToEntity(ComponentDto dto, bool isCreate = false)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (isCreate && dto.ComponentID != 0)
                throw new ArgumentException("ComponentID should not be set for new Component entities.");

            var entity = new Component
            {
                ComponentName = dto.ComponentName,
                TranslationID = dto.TranslationID,
                Translation = new TranslationGroup
                {
                    Translations = dto.DisplayNames.Select(TranslationMapper.ToEntity).ToList()
                }
            };

            if (!isCreate)
                entity.ComponentID = dto.ComponentID; // 用於更新時保留原始 ID

            return entity;
        }

    }

    // Customer
    public partial class CustomerDto
    {
        public int CustomerID { get; set; }

        [Range(0, 10000.00, ErrorMessage = "Diameter must be between 0.01 and 10000.00.")]
        public decimal Discount { get; set; }

        public int TranslationID { get; set; }
        public List<TranslationDto> DisplayNames { get; set; } = new();
    }
    public static class CustomerMapper
    {
        public static CustomerDto ToDto(Customer entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var dtoTranslations = entity.Translation.Translations
            .Select(TranslationMapper.ToDto)
            .ToList();

            return new CustomerDto
            {
                Discount = entity.Discount,
                CustomerID = entity.CustomerID,
                TranslationID = entity.TranslationID,
                DisplayNames = dtoTranslations
            };
        }
        public static Customer ToEntity(CustomerDto dto, bool isCreate = false)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (isCreate && dto.CustomerID != 0)
                throw new ArgumentException("CustomerID should not be set for new Customer entities.");

            var entity = new Customer
            {
                Discount = dto.Discount,
                TranslationID = dto.TranslationID,
                Translation = new TranslationGroup
                {
                    Translations = dto.DisplayNames.Select(TranslationMapper.ToEntity).ToList()
                }
            };

            if (!isCreate)
                entity.CustomerID = dto.CustomerID;

            return entity;
        }
    }

    // PipeTop
    public partial class PipeTopDto
    {
        public int PipeTopID { get; set; }

        [StringLength(100)][Required]
        public string PipeTopName { get; set; } = null!;
        public PipeTopPricingMethodDto PricingMethod { get; set; } = null!;
        public int TranslationID { get; set; }
        public List<TranslationDto> DisplayNames { get; set; } = new();
    }
    public static class PipeTopMapper
    {
        public static PipeTopDto ToDto(PipeTop entity)
        {
            if (entity == null) throw new ArgumentNullException("I am null.", nameof(entity));

            var dtoTranslations = entity.Translation.Translations
            .Select(TranslationMapper.ToDto)
            .ToList();

            return new PipeTopDto
            {
                PipeTopID = entity.PipeTopID,
                PipeTopName = entity.PipeTopName,
                TranslationID = entity.TranslationID,
                DisplayNames = entity.Translation.Translations.Select(TranslationMapper.ToDto).ToList(),
                PricingMethod = PipeTopPricingMethodMapper.ToDto(entity.PricingMethodNavigation)
            };
        }

        public static PipeTop ToEntity(PipeTopDto dto, bool isCreate = false)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (isCreate && dto.PipeTopID != 0)
                throw new ArgumentException("PipeTopID should not be set for new PipeTop entities.");

            var entity = new PipeTop
            {
                PipeTopName = dto.PipeTopName,
                PricingMethodNavigation = PipeTopPricingMethodMapper.ToEntity(dto.PricingMethod),
                TranslationID = dto.TranslationID,
                Translation = new TranslationGroup
                {
                    Translations = dto.DisplayNames.Select(TranslationMapper.ToEntity).ToList()
                }
            };

            if (!isCreate)
                entity.PipeTopID = dto.PipeTopID;

            return entity;
        }
    }

    // PricingMethod
    public partial class PipeTopPricingMethodDto
    {
        public int MethodID { get; set; }

        [StringLength(100)][Required]
        public string MethodName { get; set; } = null!;
        public int TranslationID { get; set; }
        public List<TranslationDto> DisplayNames { get; set; } = new();
    }
    public static class PipeTopPricingMethodMapper
    {
        public static PipeTopPricingMethodDto ToDto(PipeTopPricingMethod entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var dtoTranslations = entity.Translation.Translations
            .Select(TranslationMapper.ToDto)
            .ToList();

            return new PipeTopPricingMethodDto
            {
                MethodID = entity.MethodID,
                MethodName = entity.MethodName,
                TranslationID = entity.TranslationID,
                DisplayNames = dtoTranslations
            };
        }
        public static PipeTopPricingMethod ToEntity(PipeTopPricingMethodDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new PipeTopPricingMethod
            {
                MethodID = dto.MethodID,
                MethodName = dto.MethodName,
                TranslationID = dto.TranslationID,
                Translation = new TranslationGroup
                {
                    Translations = dto.DisplayNames.Select(TranslationMapper.ToEntity).ToList()
                }
            };
        }
    }

    // Operator
    public partial class OperatorDto
    {
        public int OperatorID { get; set; }
        public string DisplayName { get; set; } = null!;
        public string OperatorName { get; set; } = null!;
    }
    public static class OperatorMapper
    {
        public static OperatorDto ToDto(Operator entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new OperatorDto
            {
                OperatorID = entity.OperatorID,
                DisplayName = entity.DisplayName,
                OperatorName = entity.OperatorName
            };
        }
        public static Operator ToEntity(OperatorDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Operator
            {
                OperatorID = dto.OperatorID,
                DisplayName = dto.DisplayName,
                OperatorName = dto.OperatorName
            };
        }
    }


    // Stud
    public partial class StudDto
    {
        public int StudID { get; set; }
        public string DisplayName { get; set; } = null!;
        public decimal Price { get; set; }
    }
    public static class StudMapper
    {
        public static StudDto ToDto(Stud entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new StudDto
            {
                StudID = entity.StudID,
                DisplayName = entity.DisplayName,
                Price = entity.Price
            };
        }
        public static Stud ToEntity(StudDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Stud
            {
                StudID = dto.StudID,
                DisplayName = dto.DisplayName,
                Price = dto.Price
            };
        }
    }


    // PipePrice
    public partial class PipePriceDto()
    {
        public int DiameterID { get; set; }
        public int ThicknessID { get; set; }

        [Range(0.01, 10000.00, ErrorMessage = "Diameter must be between 0.01 and 10000.00.")]
        public decimal PipeUnitPrice { get; set; }
    }
    public static class PipePriceMapper
    {
        public static PipePriceDto ToDto(PipePrice entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new PipePriceDto
            {
                DiameterID = entity.DiameterID,
                ThicknessID = entity.ThicknessID,
                PipeUnitPrice = entity.PipeUnitPrice
            };
        }

        public static PipePrice ToEntity(PipePriceDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var entity = new PipePrice
            {
                DiameterID = dto.DiameterID,
                ThicknessID = dto.ThicknessID,
                PipeUnitPrice = dto.PipeUnitPrice
            };

            return entity;
        }
        
    }


    // ComponentPrice
    public partial class ComponentPriceDto()
    {
        public int DiameterID { get; set; }
        public int ComponentID { get; set; }

        [Range(0.01, 10000.00, ErrorMessage = "ComponentPrice must be between 0.01 and 10000.00.")]
        public decimal Price { get; set; }
    }
    public static class ComponentPriceMapper
    {
        public static ComponentPriceDto ToDto(ComponentPrice entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new ComponentPriceDto
            {
                DiameterID = entity.DiameterID,
                ComponentID = entity.ComponentID,
                Price = entity.Price
            };
        }

        public static ComponentPrice ToEntity(ComponentPriceDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var entity = new ComponentPrice
            {
                DiameterID = dto.DiameterID,
                ComponentID = dto.ComponentID,
                Price = dto.Price
            };

            return entity;
        }

    }
    
    // PipeTopPrice
    public partial class PipeTopPriceDto
    {
        public int DiameterID { get; set; }
        public int PipeTopID { get; set; }
        [Range(0.01, 10000.00, ErrorMessage = "PipeTopPrice must be between 0.01 and 10000.00.")]
        public decimal Price { get; set; }
    }
    public static class PipeTopPriceMapper
    {
        public static PipeTopPriceDto ToDto(PipeTopPrice entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new PipeTopPriceDto
            {
                DiameterID = entity.DiameterID,
                PipeTopID = entity.PipeTopID,
                Price = entity.Price
            };
        }
        public static PipeTopPrice ToEntity(PipeTopPriceDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var entity = new PipeTopPrice
            {
                DiameterID = dto.DiameterID,
                PipeTopID = dto.PipeTopID,
                Price = dto.Price
            };
            return entity;
        }
    }

    // ProductPipeTopMapping
    public partial class ProductPipeTopMappingDto
    {
        public int ProductID { get; set; }
        public int PipeTopID { get; set; }
    }
    public static class ProductPipeTopMappingMapper
    {
        public static ProductPipeTopMappingDto ToDto(ProductPipeTopMapping entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new ProductPipeTopMappingDto
            {
                ProductID = entity.ProductID,
                PipeTopID = entity.PipeTopID
            };
        }
        public static ProductPipeTopMapping ToEntity(ProductPipeTopMappingDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new ProductPipeTopMapping
            {
                ProductID = dto.ProductID,
                PipeTopID = dto.PipeTopID,
                IsExist = true

            };
        }
    }



}
