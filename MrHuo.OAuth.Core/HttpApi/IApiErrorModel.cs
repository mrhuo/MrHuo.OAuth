using System;
using System.Collections.Generic;
using System.Text;

namespace MrHuo.OAuth
{
    public interface IApiErrorModel
    {
        /// <summary>
        /// 错误的详细描述
        /// </summary>
        string ErrorMessage { get; set; }
    }
}
