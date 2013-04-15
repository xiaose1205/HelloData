using System;
using System.Collections.Generic;
using System.Text;

namespace HelloData.FWCommon.AOP.Metadata
{
    public class ParameterMetadata
    {
        private object _para;

        public ParameterMetadata(object para)
        {
            _para = para;
        }

        public virtual object Para
        {
            get;
            set;
        }
    }
}
