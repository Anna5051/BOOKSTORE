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
    
    public partial class Supplies
    {
        public int SupplyID { get; set; }
        public Nullable<int> BookID { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> SupplyDate { get; set; }
    
        public virtual Books Books { get; set; }
        public virtual Suppliers Suppliers { get; set; }
        public virtual Warehouses Warehouses { get; set; }
    }
}
