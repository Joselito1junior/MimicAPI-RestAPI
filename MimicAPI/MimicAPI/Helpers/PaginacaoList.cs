﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers
{
    public class PaginacaoList<T> : List<T>
    {
        public Paginacao Paginacao { get; set; }
    }
}
