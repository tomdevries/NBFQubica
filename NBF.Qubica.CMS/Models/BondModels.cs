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
    public class FederationModel
    {
        public long Id { get; set; }

        [DataType(DataType.Text)]
        [StringLength(2048, ErrorMessage = "De informatie mag maximaal 2048 posities lang zijn.")]
        [Display(Name = "Informatie")]
        public string Information { get; set; }

        [Display(Name = "Url Logo")]
        [StringLength(100, ErrorMessage = "De Url van het logo mag maximaal 100 posities lang zijn.")]
        public string UrlLogo { get; set; }
    }
}
