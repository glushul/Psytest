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
    
    public partial class Point
    {
        public int AnswerId { get; set; }
        public int CategoryId { get; set; }
        public int PointSum { get; set; }
    
        public virtual Answer Answer { get; set; }
        public virtual Category Category { get; set; }
    }
}
