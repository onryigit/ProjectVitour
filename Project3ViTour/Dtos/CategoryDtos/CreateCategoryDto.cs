using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Project3ViTour.Dtos.CategoryDtos
{
    public class CreateCategoryDto
    {
        
        public string CategoryName { get; set; }
        public bool CategoryStatus { get; set; }
    }
}
