using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace NBF.Qubica.CMS.Models
{
    public class CompetitionGridModel
    {
        [Display(Name = "Nummer")]
        public long Id { get; set; }

        public long challengeId { get; set; }

        [Display(Name = "Competitie")]
        public string challenge { get; set; }

        [Display(Name = "Omschrijving")]
        public string description { get; set; }

        [Display(Name = "Prijs")]
        public string price { get; set; }

        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Einde")]
        public DateTime EndDate { get; set; }
    }

    public class CompetitionModel
    {
        public long Id { get; set; }

        public long challengeId { get; set; }

        [Display(Name = "Competitie")]
        public string challenge { get; set; }

        [Display(Name = "Omschrijving")]
        public string description { get; set; }

        [Display(Name = "Prijs")]
        public string price { get; set; }

        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Einde")]
        public DateTime EndDate { get; set; }

        public IEnumerable<C_Checkbox> AvailableBowlingCenters { get; set; }
        public IEnumerable<C_Checkbox> SelectedBowlingCenters { get; set; }
        public C_PostedCheckbox PostedBowlingCenters { get; set; }

        public bool AllBowlingCentersChecked { get; set; }
    }

    public class PlayerGridModel
    {
        [Display(Name = "Nummer")]
        public long Id { get; set; }

        public long competitionId { get; set; }

        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Display(Name = "Frequent Bowlernummer")]
        public long FrequentBowlernumber { get; set; }
    }
}