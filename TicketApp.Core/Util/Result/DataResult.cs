using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using TicketApp.Application.Models.Response;
using TicketApp.Core.Entities.Base;
using TicketApp.Core.Util.Result;
using TicketApp.Infrastructure.Extension;

namespace TicketApp.Core.Util.Result
{
    public class DataResult : IDisposable
    {
        private BaseResponseModel _data;
        private bool _isSuccesful;

        public Error? Error { get; set; }
        public int HttpStatusCode { get; set; }

        public bool IsScucceful
        {
            get { return _isSuccesful; }
            private set
            {
                _isSuccesful = value;
                if (this.Error != null)
                {
                    _isSuccesful = false;
                }
                else
                {
                    _isSuccesful = true;
                }
            }
        }

        public BaseResponseModel? Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public override string ToString()
        {
            return this.ToJSON();
        }

        public void Dispose()
        {
        }
    }
}