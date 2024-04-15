using System.ComponentModel.DataAnnotations;

namespace ADODomainModels.models
{
    public class GitHttpClientModel
    {
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string RepositoryName { get; set; }
        [Required]
        public string OrganizationUrl { get; set; }
        [Required]
        public string PersonalAccessToken { get; set; }
        public DateTime ? StartDate { get; set; }

    }
}
