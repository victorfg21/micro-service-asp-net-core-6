using System.ComponentModel.DataAnnotations;

namespace GeekShopping.Web.Models
{
    public class ProductModel
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public string? ImageUrl { get; set; }

        [Range(1, 100)]
        public int Count { get; set; } = 1;

        public string SubstringDescription()
        {
            if (Description?.Length < 355) return Name ?? string.Empty;
            return $"{Description?[..352]} ...";
        }
    }
}
