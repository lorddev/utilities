using System.Collections.Generic;

namespace LordDesign.Utilities.MapsApi
{
    public class ElementRow
    {
        #region Constructors and Destructors

        public ElementRow()
        {
            Elements = new List<DistanceElement>();
        }

        #endregion

        #region Public Properties

        public ICollection<DistanceElement> Elements { get; set; }

        #endregion
    }
}