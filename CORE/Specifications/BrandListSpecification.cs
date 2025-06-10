using System;
using CORE.Entities;

namespace CORE.Specifications;

public class BrandListSpecification : BaseSpecification<Product,string>
{
    public BrandListSpecification()
    {
        AddSelect(x => x.Brand);
        ApplyDistinct();
    }
}
