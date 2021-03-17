using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AYP.Models
{
    public class ResponseModel
    {
        [JsonProperty(Required = Required.Always)]
        public bool HasError { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Message { get; set; }
        public long Id { get; set; }

        public ResponseModel()
        {
            HasError = false;
            Message = "";
        }

        public ResponseModel SetError(string message = null)
        {
            HasError = true;
            Message = message ?? "Server Error";

            return this;
        }

        public T SetError<T>(string message) where T : ResponseModel
        {
            HasError = true;
            Message = message;

            return (T)this;
        }

        public static ResponseModel GetError(string message)
        {
            var ret = new ResponseModel();
            ret.SetError(message);
            return ret;
        }

        public static T GetError<T>(string message) where T : ResponseModel, new()
        {
            var ret = new T();
            ret.SetError(message);
            return (T)ret;
        }

        public ResponseModel SetSuccess(string message = "İşlem Başarı ile Gerçekleştirildi.")
        {
            HasError = false;
            Message = message;

            return this;
        }

        public T SetSuccess<T>(string message = "İşlem Başarı ile Gerçekleştirildi.") where T : ResponseModel
        {
            HasError = false;
            Message = message;

            return (T)this;
        }

        public static ResponseModel GetSuccess(string message = "")
        {
            var ret = new ResponseModel();
            ret.SetSuccess(message);
            return ret;
        }

        public static T GetSuccess<T>(string message = "") where T : ResponseModel, new()
        {
            var ret = new T();
            ret.SetSuccess(message);
            return (T)ret;
        }
    }
}
