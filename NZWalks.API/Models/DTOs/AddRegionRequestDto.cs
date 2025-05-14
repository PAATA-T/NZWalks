using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code has to be a minimum 3 characters")]
        [MaxLength(3, ErrorMessage ="Code has to be a maximum 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Code has to be a minimum 50 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
