//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Psytest.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Raging
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int MinSum { get; set; }
        public int MaxSum { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int FontStyle { get; set; }
    
        public virtual Category Category { get; set; }
    }
}