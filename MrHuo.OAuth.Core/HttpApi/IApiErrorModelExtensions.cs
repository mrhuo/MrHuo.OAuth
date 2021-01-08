using System;
using System.Collections.Generic;
using System.Text;

namespace MrHuo.OAuth
{
    public static class IApiErrorModelExtensions
    {
        /// <summary>
        /// 是否包含错误
        /// </summary>
        /// <param name="apiErrorModel"></param>
        /// <returns></returns>
        public static bool HasError(this IApiErrorModel apiErrorModel)
        {
            return !string.IsNullOrEmpty(apiErrorModel.ErrorMessage);
        }
    }
}
