//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppTuotteetMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class KategoriaMyynti
    {
        [Key]
        public long rowid { get; set; }
        public string KategoriaNimi { get; set; }
        public string Nimi { get; set; }
        public Nullable<decimal> TuoteMyynnit { get; set; }
    }
}
