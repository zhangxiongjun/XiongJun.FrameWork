﻿using System;
using System.Collections.Generic;
using System.Text;

namespace XiongJun.Core.Infrastructure
{
    public interface IMapperProfile
    {
        /// <summary>
        /// Gets order of this configuration implementation
        /// </summary>
        int Order { get; }
    }
}
