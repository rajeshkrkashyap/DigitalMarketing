using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class PurposeAndIntent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ContentQualityId { get; set; }
        public int EducationAndInformativeCount { get; set; }
        public int EntertainingCount { get; set; }
        public int PromotionalCount { get; set; }
        public int TransactionalAndECommerceCount { get; set; }
        public int NewsAndUpdatesCount { get; set; }
        public int SocialInteractionAndCommunityBuildingCount { get; set; }
        public int EducationalInstitutionsTrainingCount { get; set; }
        public int LegalAndPolicyCount { get; set; }
        public int ReviewsAndTestimonialsCount { get; set; }
        public int ResourceAndReferenceCount { get; set; }
        public int SupportAndHelp { get; set; }
        public int BlogAndOpinion { get; set; }
        public bool IsActive { get; set; }
        public virtual ContentQuality? ContentQuality { get; set; }

    }
}
