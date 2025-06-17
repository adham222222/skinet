using System;
using API.DTO;
using CORE.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace API.Extensions;

public static class AddressMappingExtensions
{
    public static AddressDTO? ToDTO(this Address address)
    {
        if (address == null) return null;

        return new AddressDTO
        {
            Line1 = address.Line1,
            Line2 = address.Line2,
            City = address.City,
            Country = address.Country,
            PostalCode = address.PostalCode,
            State = address.State
        };
    }
    public static Address ToEntity(this AddressDTO address)
    {
        if (address == null) throw new ArgumentException(nameof(AddressDTO));

        return new Address
        {
            Line1 = address.Line1,
            Line2 = address.Line2,
            City = address.City,
            Country = address.Country,
            PostalCode = address.PostalCode,
            State = address.State
        };
    }
    
     public static void  UpdateDTO(this Address address,AddressDTO addressDTO)
    {
        if (address == null) throw new ArgumentException(nameof(Address));
        
        if (addressDTO == null) throw new ArgumentException(nameof(AddressDTO));



        address.Line1 = addressDTO.Line1;
        address.Line2 = addressDTO.Line2;
        address.City = addressDTO.City;
        address.Country = addressDTO.Country;
        address.PostalCode = addressDTO.PostalCode;
        address.State = addressDTO.State;
        
    }
}
