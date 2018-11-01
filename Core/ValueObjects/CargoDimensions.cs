using System;
using System.Collections.Generic;

namespace Core.ValueObjects
{
    public class CargoDimensions : IEquatable<CargoDimensions>
    {
        protected CargoDimensions() { }

        public CargoDimensions(decimal? weight, decimal? volume, int? numberOfPackages, UOM? unitOfMeasure)
        {
            Weight = weight;
            Volume = volume;
            NumberOfPackages = numberOfPackages;
            UnitOfMeasure = unitOfMeasure;
        }

        public decimal? Weight { get; private set; }
        public decimal? Volume { get; private set; }
        public int? NumberOfPackages { get; private set; }
        public UOM? UnitOfMeasure { get; private set; }

        #region Equality
        public override bool Equals(object obj)
        {
            return Equals(obj as CargoDimensions);
        }

        public bool Equals(CargoDimensions other)
        {
            return other != null &&
                   EqualityComparer<decimal?>.Default.Equals(Weight, other.Weight) &&
                   EqualityComparer<decimal?>.Default.Equals(Volume, other.Volume) &&
                   EqualityComparer<int?>.Default.Equals(NumberOfPackages, other.NumberOfPackages) &&
                   EqualityComparer<UOM?>.Default.Equals(UnitOfMeasure, other.UnitOfMeasure);
        }

        public override int GetHashCode()
        {
            var hashCode = -1269697782;
            hashCode = hashCode * -1521134295 + EqualityComparer<decimal?>.Default.GetHashCode(Weight);
            hashCode = hashCode * -1521134295 + EqualityComparer<decimal?>.Default.GetHashCode(Volume);
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(NumberOfPackages);
            hashCode = hashCode * -1521134295 + EqualityComparer<UOM?>.Default.GetHashCode(UnitOfMeasure);
            return hashCode;
        }

        public static bool operator ==(CargoDimensions dimensions1, CargoDimensions dimensions2)
        {
            return EqualityComparer<CargoDimensions>.Default.Equals(dimensions1, dimensions2);
        }

        public static bool operator !=(CargoDimensions dimensions1, CargoDimensions dimensions2)
        {
            return !(dimensions1 == dimensions2);
        }
        #endregion
    }
}
