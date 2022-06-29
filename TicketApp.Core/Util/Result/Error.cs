using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using TicketApp.Infrastructure.Extension;

namespace TicketApp.Core.Util.Result
{
    public class Error : IDisposable
    {
        private List<ValidationFailure>? _validationResult = new List<ValidationFailure>();

        public Error()
        {
        }

        public Error(string application, string operation, List<string> errorMessageList,
            int errorCode)
        {
            Application = application;
            Operation = operation;
            ErrorMessageList = errorMessageList;
            ErrorCode = errorCode;
        }

        public string? Application { get; set; }
        public string? Operation { get; set; }
        public List<string>? ErrorMessageList { get; set; } = new List<string> { };
        public int? ErrorCode { get; set; }

        /*public List<ValidationFailure>? ValidationResult
        {
            get
            {
                if (_validationResult != null && _validationResult.Count > 0)
                    return _validationResult;
                else
                {
                    List<ValidationFailure> tempValidFailList = new List<ValidationFailure>();
                    if (ErrorMessageList.Any())
                    {
                        foreach (string errorItem in ErrorMessageList)
                        {
                            tempValidFailList.Add(new ValidationFailure("", "")
                            {
                                ErrorMessage = errorItem
                            });
                        }
                    }

                    _validationResult = tempValidFailList;
                    return _validationResult;
                }
            }
            set { this._validationResult = value; }
        }*/

        public override string ToString()
        {
            return this.ToJSON();
        }

        public void Dispose()
        {
        }
    }
}