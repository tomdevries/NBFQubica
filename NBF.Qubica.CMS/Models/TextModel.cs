using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NBF.Qubica.CMS.Models
{
    public class TextGridModel
    {
        [Display(Name = "Nummer")]
        public long Id { get; set; }

        [Display(Name = "Label")]
        public string Label { get; set; }
    }

    public class TextModel
    {
        public long Id { get; set; }

        [Display(Name = "Label")]
        public string label { get; set; }

        [Display(Name = "Tekst")]
        public string text { get; set; }
    }
}