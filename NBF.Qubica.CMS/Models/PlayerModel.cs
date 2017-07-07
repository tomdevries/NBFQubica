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
    public class PlayerModel
    {
        [Display(Name = "Nummer")]
        public long Id { get; set; }

        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Display(Name = "Frequent Bowlernummer")]
        public long FrequentBowlernumber { get; set; }
    }
}