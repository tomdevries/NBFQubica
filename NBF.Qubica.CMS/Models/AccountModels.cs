using NBF.Qubica.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace NBF.Qubica.CMS.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage="De ID-Naam is verplicht")]
        [Display(Name = "ID-Naam")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Het ID-Nummer is verplicht")]
        [Display(Name = "ID-nummer")]
        public long FrequentBowlerNumber { get; set; }

        [Required(ErrorMessage = "Het wachtwoord is verplicht")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "De naam is verplicht")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Het e-mailadres is verplicht")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Het adres is verplicht")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "De plaats is verplicht")]
        [Display(Name = "Plaats")]
        public string City { get; set; }
        
        [Required(ErrorMessage = "De ID-Naam is verplicht")]
        [Display(Name = "ID-Naam")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Het wachtwoord is verplicht")]
        [StringLength(100, ErrorMessage = "Het {0} moet minimaal {2} tekens lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord bevestiging")]
        [Compare("Password", ErrorMessage = "Het wachtwoord en het bevestigde wachtwoord komen niet overeen.")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Is lid van een bowlingbond")]
        public bool IsMember { get; set; }
    
        [Display(Name = "Lidmaatschapnummer")]
        public int MemberNumber { get; set; }

        [Display(Name = "ID-Nummer")]
        public long FrequentBowlerNumber { get; set; }
    }

    public class AccountModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "De naam is verplicht")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Het e-mailadres is verplicht")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Het adres is verplicht")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "De plaats is verplicht")]
        [Display(Name = "Plaats")]
        public string City { get; set; }

        [Required(ErrorMessage = "De ID-Naam is verplicht")]
        [Display(Name = "ID-Naam")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord bevestiging")]
        [Compare("Password", ErrorMessage = "Het wachtwoord en het bevestigde wachtwoord komen niet overeen.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "ID-Nummer")]
        public long FrequentBowlerNumber { get; set; }

        [Display(Name = "Rol")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Is lid van een bowlingbond")]
        public bool IsMember { get; set; }

        [Display(Name = "Lidmaatschapnummer")]
        public int? MemberNumber { get; set; }

        [Display(Name = "Is lid van een bowlingbond")]
        public bool IsRegistrationConfirmed { get; set; }       
    }

    public class AccountGridModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public long FrequentBowlerNumber { get; set; }
    }
}
