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
    public class BowlinghuisModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "De naam is verplicht")]
        [StringLength(45, ErrorMessage = "De naam mag maximaal 45 posities lang zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required(ErrorMessage = "De uri is verplicht")]
        [StringLength(150, ErrorMessage = "De uri mag maximaal 150 posities lang zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Uri")]
        public string Uri { get; set; }

        [Required(ErrorMessage = "De center ID is verplicht")]
        [Range(1024, 32786, ErrorMessage = "1024-32767")]
        [RegularExpression(@"[0-9]{4}$", ErrorMessage = "Geef een center ID in")]
        [Display(Name = "Center ID")]
        public int Port { get; set; }

        [Required(ErrorMessage = "De versie is verplicht")]
        [Display(Name = "Versie")]
        public string ApiVersion { get; set; }
        public IEnumerable<SelectListItem> ApiVersions { get; set; }

        [Required(ErrorMessage = "Het aantal banen is verplicht")]
        [Range(1, 64, ErrorMessage = "1-64")]
        [RegularExpression(@"[0-9]{1,2}$", ErrorMessage = "Geef het aantal banen in tussen 1 en 64")]
        [Display(Name = "Aantal banen")]
        public int NumberOfLanes { get; set; }

        [Display(Name = "Laatste synchronisatie datum")]
        public DateTime LastSyncDate { get; set; }

        [Display(Name = "Adres")]
        [StringLength(45, ErrorMessage = "Het adres mag maximaal 45 posities lang zijn.")]
        public string Address { get; set; }

        [Display(Name = "Postcode")]
        [StringLength(6, ErrorMessage = "De postcode mag maximaal 6 posities lang zijn.")]
        [RegularExpression(@"[0-9]{4}[A-Z]{2}$", ErrorMessage = "De postcode moet bestaan uit 4 cijfers en 2 hoofdletters.")]
        public string ZipCode { get; set; }

        [Display(Name = "Plaats")]
        [StringLength(45, ErrorMessage = "De plaats mag maximaal 45 posities lang zijn.")]
        public string City { get; set; }

        [Display(Name = "Telefoonnummer")]
        [StringLength(10, ErrorMessage = "Het telefoonummer mag maximaal 10 posities lang zijn.")]
        [RegularExpression(@"0[0-9]{9}$", ErrorMessage = "Het telefoonnummer moet beginnen met een 0 en 10 positites lang zijn.")]
        public string Phonenumber { get; set; }

        [Display(Name = "AppName")]
        [StringLength(45, ErrorMessage = "De applicatienaam mag maximaal 45 posities lang zijn.")]
        public string Appname { get; set; }

        [Display(Name = "Secretkey")]
        [StringLength(45, ErrorMessage = "De secretkey mag maximaal 45 posities lang zijn.")]
        public string Secretkey { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(45, ErrorMessage = "De plaats mag maximaal 45 posities lang zijn.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage= "Het email adres voldoet niet aan de email standaard.")]
        public string Email { get; set; }

        [Display(Name = "Website")]
        [StringLength(100, ErrorMessage = "Het website adres mag maximaal 100 posities lang zijn.")]
        public string Website { get; set; }

        [Display(Name = "Url Logo")]
        [StringLength(100, ErrorMessage = "De Url van het logo mag maximaal 100 posities lang zijn.")]
        public string UrlLogo { get; set; }
    }

    public class BowlinghuisGridModel
    {
        public long id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; } 
        public string City { get; set; }
        public string Phonenumber { get; set; }
        public string Appname { get; set; }
        public string SecretKey { get; set; }
    }

    public class OpentimeModel
    {
        public long Id { get; set; }
        public long BowlingcenterId { get; set; }
        public string Day { get; set; }
        public IEnumerable<SelectListItem> Days { get; set; }
        public String Opentime { get; set; }
        public String Closetime { get; set; }
    }

    public class OpentimeGridModel
    {
        public long Id { get; set; }
        public long BowlingcenterId { get; set; }
        public string Day { get; set; }
        public String Opentime { get; set; }
        public String Closetime { get; set; }
    }

    public class AdvertModel
    {
        public long Id { get; set; }
        public long BowlingcenterId { get; set; }

        [Display(Name = "Omschrijving")]
        [Required(ErrorMessage = "De omschrijving is verplicht")]
        public string Advertisement { get; set; }

        [Display(Name = "Link naar banner")]
        public String AdvertisementUrl { get; set; }

        [Display(Name = "Website adres")]
        [Required(ErrorMessage = "De website is verplicht")]
        public String AdvertisementWWW { get; set; }
    }

    public class AdvertGridModel
    {
        public long Id { get; set; }
        public long BowlingcenterId { get; set; }
        public string Advertisement { get; set; }
        public String AdvertisementUrl { get; set; }
        public String AdvertisementWWW { get; set; }
    }
}
