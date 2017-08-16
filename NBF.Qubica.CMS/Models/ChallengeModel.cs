using System.ComponentModel.DataAnnotations;

namespace NBF.Qubica.CMS.Models
{
    public class ChallengeGridModel
    {
        [Display(Name = "Nummer")]
        public long Id { get; set; }

        [Display(Name = "Naam")]
        public string Name { get; set; }
    }
}