//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace piquedo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ImgRecord
    {
        public string Id { get; set; }
        public string url { get; set; }
        public string PostingUserID { get; set; }
        public string WorkId { get; set; }
        public string Description { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Work Work { get; set; }
    }
}
