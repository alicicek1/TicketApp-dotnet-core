using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using TicketApp.Core.Entities.Base;

namespace TicketApp.Core.Util.Result
{
    public class DataResult : ApiResult
    {
        private IDocument _data;

        public string PkId { get; set; }

        public IDocument Data
        {
            get { return _data; }
            set
            {
                _data = value;
                if (_data != null)
                {
                    this.IsSuccessful = true;
                    var findPkIdAttribute = _data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(a => a.GetCustomAttributes<BsonIdAttribute>() != null).GetValue(_data);
                    if (findPkIdAttribute != null)
                    {
                        this.PkId = findPkIdAttribute.ToString();
                    }
                }
                else if (!IsSuccessful && !this.ErrorMessageList.Any())
                {
                    this.ErrorMessageList = new List<string> {"Not found."};
                }
            }
        }
    }
}