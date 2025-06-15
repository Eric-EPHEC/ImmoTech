namespace Immotech.Front.Models;

// Enum representing all possible property types handled by the backend.
// Keep exactly same names as backend so that serialized strings match.
public enum PropertyType
{
    House,
    Apartment,
    Land,
    Garage,
    Office,
    Shop,
    Warehouse,
    ApartmentBuilding,
    Hotel,
    Other
} 