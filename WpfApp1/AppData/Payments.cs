//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1.AppData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Payments
    {
        public int PaymentID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string PaymentMethod { get; set; }
    
        public virtual Orders Orders { get; set; }
    }
}
