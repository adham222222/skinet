using System;
using System.Security.Cryptography.X509Certificates;
using CORE.Entities;

namespace CORE.Specifications;

public class TypeListSpecification : BaseSpecification<Product,string>
{
     public TypeListSpecification()
     {
        AddSelect(x=>x.Type);
        ApplyDistinct();
     }
}
