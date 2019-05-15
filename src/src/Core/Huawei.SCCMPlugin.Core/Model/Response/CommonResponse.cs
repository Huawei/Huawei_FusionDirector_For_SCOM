using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.Core.Model.Response
{
    /// <summary>
    /// Class CommonResponse.
    /// </summary>
    public class CommonResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonResponse"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public CommonResponse(string code)
        {
            this.Code = code;
            this.Data = new List<Item>();
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [JsonProperty("data")]
        public List<Item> Data { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("headers")]

        public object Headers { get; set; }
    }

    /// <summary>
    /// Class Item.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
